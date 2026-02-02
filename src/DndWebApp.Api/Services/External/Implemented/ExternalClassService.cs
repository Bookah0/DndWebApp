namespace DndWebApp.Api.Services.External.Implemented;

using System.Text.Json;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.ExternalDTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items.Constants;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using static DndWebApp.Api.Services.Util.ConstantsUtil;

public class ExternalClassService(IClassRepository classRepo, ISubclassRepository subclassRepo, IAbilityRepository abilityRepo, IItemRepository itemRepository, ILogger<ExternalClassService> logger) : IExternalClassService
{
    private readonly HttpClient client = new();

    public async Task FetchExternalClassesAsync(CancellationToken cancellationToken = default)
    {
        if ((await classRepo.GetAllAsync()).Count > 0)
        {
            logger.LogInformation("Classes already exist in the database. Skipping fetch.");
        }

        logger.LogInformation("Fetching external classes.");

        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/classes/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
            throw new InvalidOperationException("No classes found in external API.");

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/classes/{item.Index}", cancellationToken);
            var eClass = await JsonSerializer.DeserializeAsync<EClassDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eClass is null)
                throw new InvalidOperationException($"Failed to deserialize class {item.Index}.");

            Ability? spellcastingAbility = null;

            if (eClass.SpellcastingAbility is not null)
            {
                spellcastingAbility = await abilityRepo.GetByNameAsync(eClass.SpellcastingAbility.SpellcastingAbility.Name)
                    ?? throw new ArgumentException($"Ability with short name {eClass.SpellcastingAbility.SpellcastingAbility.Index} not found.");
            }

            var clss = new Class
            {
                Name = eClass.Name,
                Description = "",
                HitDie = eClass.HitDie,
                ClassLevels = [],
                SpellcastingAbilityId = spellcastingAbility?.Id ?? null,
                Subclasses = [],
                StartingEquipment = [],
                StartingEquipmentChoices = []
            };

            await classRepo.CreateAsync(clss);

            await AddStartingEquipmentAsync(eClass, clss, classRepo);
            await AddEquipmentChoicesAsync(eClass, clss, classRepo);
            await FetchExternalClassFeaturesAsync(cancellationToken);
            await FetchExternalClassLevelsAsync(clss, cancellationToken);
            await FetchExternalSubclassesAsync(clss, eClass.Subclasses, cancellationToken);

            await classRepo.UpdateAsync(clss);
        }

        logger.LogInformation("Successfully fetched external classes. Count: {ClassCount}", result.Results.Count);
    }

    // Class levels based on https://www.dnd5eapi.co/api/2014/classes/{class}/levels and https://www.dnd5eapi.co/api/2014/subclasses/{subclass}/levels 
    public async Task FetchExternalClassLevelsAsync(AClass clss, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching external class levels, Name: {ClassName}, ID: {ClassId}", clss.Name, clss.Id);
        HttpResponseMessage getListResponse;

        if (clss is Class)
        {
            getListResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/classes/{clss.Name}/levels", cancellationToken);
        }
        else
        {
            getListResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/subclasses/{clss.Name}/levels", cancellationToken);
        }

        var result = await JsonSerializer.DeserializeAsync<List<EClassLevelDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Count == 0)
            throw new InvalidOperationException("No classes found in external API.");

        var abilityScoreIncreaseChoiceList = await CreateAbilityIncreaseChoiceList();
        var currentAbilityScoreBonuses = 0;

        foreach (var level in result)
        {
            int[]? spellSlots = null;

            if (level.Spellcasting is not null)
            {
                spellSlots = [
                    level.Spellcasting.SpellSlotsLevel1 ?? 0,
                    level.Spellcasting.SpellSlotsLevel2 ?? 0,
                    level.Spellcasting.SpellSlotsLevel3 ?? 0,
                    level.Spellcasting.SpellSlotsLevel4 ?? 0,
                    level.Spellcasting.SpellSlotsLevel5 ?? 0,
                    level.Spellcasting.SpellSlotsLevel6 ?? 0,
                    level.Spellcasting.SpellSlotsLevel7 ?? 0,
                    level.Spellcasting.SpellSlotsLevel8 ?? 0,
                    level.Spellcasting.SpellSlotsLevel9 ?? 0
                ];
            }

            var ClassSpecificSlots = new List<ClassSpecificSlot>();

            var curClassLevel = new ClassLevel
            {
                Level = level.Level,
                ProficiencyBonus = level.ProficiencyBonus,
                CantripsKnown = level.Spellcasting?.CantripsKnown ?? 0,
                SpellSlots = spellSlots,
                Class = clss,
                ClassId = clss.Id,
            };

            PopulateClassSpecificSlotList(level, curClassLevel);

            currentAbilityScoreBonuses = level.AbilityScoreBonuses - currentAbilityScoreBonuses;

            if (currentAbilityScoreBonuses > 0)
            {
                curClassLevel.NewFeatures.Add(new ClassFeature
                {
                    Name = "Ability Score Improvement",
                    Description = $"When you reach {level.Level}th level, you can increase one ability score of your choice by 2, or you can increase two ability scores of your choice by 1. As normal, you can't increase an ability score above 20 using this feature.",
                    Level = curClassLevel,
                    LevelId = curClassLevel.Id,
                    ClassId = clss.Id,
                    AbilityIncreaseChoices = abilityScoreIncreaseChoiceList
                });
            }
        }

        logger.LogInformation("Successfully fetched external class levels, Name: {ClassName}, ID: {ClassId}", clss.Name, clss.Id);
    }

    // Subclasses based on https://www.dnd5eapi.co/api/2014/subclasses/
    public async Task FetchExternalSubclassesAsync(Class clss, List<EIndexDto> subclassIndexList, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching external subclasses, ClassName: {ClassName}, SubclassCount: {SubclassCount}", clss.Name, subclassIndexList.Count);
        foreach (var item in subclassIndexList)
        {
            var getListResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/subclasses/{item.Index}", cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<List<ESubclassDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (result is null || result.Count == 0)
                throw new InvalidOperationException("No subclasses found in external API.");

            foreach (var eSubclass in result)
            {
                var subclass = new Subclass
                {
                    Name = eSubclass.Name,
                    Description = string.Join("\n", eSubclass.Description),
                    HitDie = clss.HitDie,
                    ClassLevels = [],
                    ParentClass = clss,
                    ParentClassId = clss.Id
                };

                await subclassRepo.CreateAsync(subclass);
                await FetchExternalClassLevelsAsync(subclass, cancellationToken);
                await FetchExternalClassFeaturesAsync(cancellationToken);
                await subclassRepo.UpdateAsync(subclass);

                clss.Subclasses.Add(subclass);
            }
        }

        logger.LogInformation("Successfully fetched external subclasses, ClassName: {ClassName}, SubclassCount: {SubclassCount}", clss.Name, subclassIndexList.Count);
    }

    public Task FetchExternalClassFeaturesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching external class features.");
        throw new NotImplementedException();
    }

    private async Task AddStartingEquipmentAsync(EClassDto eClass, Class clss, IClassRepository classRepo)
    {
        if (eClass.StartingEquipment is null)
            return;

        foreach (var eItem in eClass.StartingEquipment)
        {
            var item = await itemRepository.GetByNameAsync(eItem.Equipment.Name)
                ?? throw new ArgumentException($"Item with name {eItem.Equipment.Name} not found.");

            clss.StartingEquipment.Add(item);
        }
    }

    private async Task AddEquipmentChoicesAsync(EClassDto eClass, Class clss, IClassRepository classRepo)
    {
        if (eClass.StartingEquipmentChoices is null)
            return;

        foreach (var eItem in eClass.StartingEquipmentChoices)
        {
            if (eItem.Options is null)
                continue;

            var choice = new StartingEquipmentChoice
            {
                Description = eItem.Description,
                NumberOfChoices = eItem.NumberOfChoices,
                Options = []
            };

            foreach (var eOption in eItem.Options.Options)
            {
                if (eOption.Equipment is not null)
                {
                    var item = await itemRepository.GetByNameAsync(eOption.Equipment.Name)
                        ?? throw new ArgumentException($"Item with name {eOption.Equipment.Name} not found.");

                    var option = new StartingEquipmentOption
                    {
                        Equipment = item,
                        Quantity = eOption?.Count ?? 1,
                    };

                    choice.Options.Add(option);
                }
                else if (eOption.EquipmentFromCategoryChoice is not null)
                {
                    var categoryDto = eOption.EquipmentFromCategoryChoice.From.EquipmentCategory;
                    var option = new StartingEquipmentOption
                    {

                        Quantity = eOption?.Count ?? 1,
                    };

                    switch (categoryDto.Index)
                    {
                        case "simple-melee-weapons":
                        case "martial-melee-weapons":
                        case "simple-ranged-weapons":
                        case "martial-ranged-weapons":
                            option.AnyOfWeaponCategory = ResolveOptionOrThrow(categoryDto.Index, WeaponCategory.AllowedValues, "Weapon Category");
                            break;
                        default:
                            option.AnyOfWeaponType = ResolveOptionOrThrow(categoryDto.Index, WeaponType.AllowedValues, "Weapon Type");
                            break;
                    }

                    choice.Options.Add(option);
                }
            }

            clss.StartingEquipmentChoices.Add(choice);
        }
    }

    private async Task<List<AbilityIncreaseChoice>> CreateAbilityIncreaseChoiceList()
    {
        var abilities = await abilityRepo.GetAllAsync();
        var abilityIncreaseChoices = new List<AbilityIncreaseChoice>();

        foreach (var ability in abilities)
        {
            var singleAbilityValue = new AbilityValue
            {
                AbilityId = ability.Id,
                Ability = ability,
                Value = 2
            };

            var choice = new AbilityIncreaseChoice
            {
                Description = $"Increase {ability.FullName} by 2",
                Options = [singleAbilityValue]
            };

            abilityIncreaseChoices.Add(choice);

            var multipleAbilityValueFirst = new AbilityValue
            {
                AbilityId = ability.Id,
                Ability = ability,
                Value = 1
            };

            foreach (var otherAbility in abilities.Where(a => a.Id != ability.Id))
            {
                var multipleAbilityValueSecond = new AbilityValue
                {
                    AbilityId = otherAbility.Id,
                    Ability = otherAbility,
                    Value = 1
                };

                choice = new AbilityIncreaseChoice
                {
                    Description = $"Increase {ability.FullName} by 1 and {otherAbility.FullName} by 1",
                    Options = [multipleAbilityValueFirst, multipleAbilityValueSecond]
                };

                abilityIncreaseChoices.Add(choice);
            }
        }

        return abilityIncreaseChoices;
    }

    private static void PopulateClassSpecificSlotList(EClassLevelDto eLevel, ClassLevel classLevel)
    {
        var cs = eLevel.ClassSpecific;

        var toMap = new List<(int? value, string name)>
        {
            (cs.ActionSurges, "Action Surges"),
            (cs.IndomitableUses, "Indomitable Uses"),
            (cs.ExtraAttacks, "Extra Attacks"),
            (cs.ArcaneRecoveryLevels, "Arcane Recovery Levels"),
            (cs.RageCount, "Rage Count"),
            (cs.RageDamageBonus, "Rage Damage Bonus"),
            (cs.KiPoints, "Ki Points"),
            (cs.UnarmoredMovement, "Unarmored Movement Bonus"),
            (cs.FavoredEnemies, "Favored Enemies"),
            (cs.FavoredTerrain, "Favored Terrain"),
            (cs.InvocationsKnown, "Invocations Known"),
            (cs.MysticArcanumLevel6, "Mystic Arcanum Level 6"),
            (cs.MysticArcanumLevel7, "Mystic Arcanum Level 7"),
            (cs.MysticArcanumLevel8, "Mystic Arcanum Level 8"),
            (cs.MysticArcanumLevel9, "Mystic Arcanum Level 9"),
            (cs.SorceryPoints, "Sorcery Points"),
            (cs.MetamagicKnown, "Metamagic Known"),
            (cs.AuraRange, "Aura Range"),
            (cs.BrutalCriticalDice, "Brutal Critical Dice"),
            (cs.BardicInspirationDie, "Bardic Inspiration Die"),
            (cs.SongOfRestDie, "Song of Rest Die"),
            (cs.MagicalSecretsMax5, "Magical Secrets Max 5"),
            (cs.MagicalSecretsMax7, "Magical Secrets Max 7"),
            (cs.MagicalSecretsMax9, "Magical Secrets Max 9"),
            (cs.ChannelDivinityCharges, "Channel Divinity Charges"),
            ((int?)cs.DestroyUndeadCr, "Destroy Undead CR"),
            ((int?)cs.WildShapeMaxCr, "Wild Shape Max CR"),
            (cs.WildShapeFly == true ? 1 : 0, "Wild Shape Fly"),
            (cs.WildShapeSwim == true ? 1 : 0 , "Wild Shape Swim")
        };

        foreach (var (value, name) in toMap)
        {
            if (value.HasValue)
            {
                classLevel.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot
                {
                    Name = name,
                    Quantity = value.Value
                });
            }
        }

        // Nested objects
        if (cs.SneakAttack is not null)
        {
            classLevel.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot
            {
                Name = "Sneak Attack Dice Count",
                Quantity = cs.SneakAttack.DiceCount
            });
            classLevel.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot
            {
                Name = "Sneak Attack Dice Value",
                Quantity = cs.SneakAttack.DiceValue
            });
        }

        if (cs.MartialArts is not null)
        {
            classLevel.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot
            {
                Name = "Martial Arts Dice Count",
                Quantity = cs.MartialArts.DiceCount
            });
            classLevel.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot
            {
                Name = "Martial Arts Dice Value",
                Quantity = cs.MartialArts.DiceValue
            });
        }

        // Collections
        if (cs.CreatingSpellSlots is not null)
        {
            foreach (var spellSlot in cs.CreatingSpellSlots)
            {
                classLevel.ClassSpecificSlotsAtLevel.Add(new ClassSpecificSlot
                {
                    Name = $"Creating Spell Slot Level {spellSlot.SpellSlotLevel}",
                    Quantity = spellSlot.SorceryPointCost
                });
            }
        }
    }
}