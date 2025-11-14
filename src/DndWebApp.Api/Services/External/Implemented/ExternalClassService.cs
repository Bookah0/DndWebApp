using System.Text.Json;
using System.Xml.Serialization;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs.ExternalDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.External.Interfaces;
using DndWebApp.Api.Services.Util;

public class ExternalClassService : IExternalClassService
{
    private readonly IClassRepository classRepo;
    private readonly ISubclassRepository subclassRepo;
    private readonly IAbilityRepository abilityRepo;
    private readonly IItemRepository itemRepository;
    private readonly HttpClient client = new();

    public ExternalClassService(IClassRepository classRepo, ISubclassRepository subclassRepo, IAbilityRepository abilityRepo, IItemRepository itemRepository)
    {
        this.classRepo = classRepo;
        this.subclassRepo = subclassRepo;
        this.abilityRepo = abilityRepo;
        this.itemRepository = itemRepository;
    }

    // Classes based on https://www.dnd5eapi.co/api/2014/classes/
    public async Task FetchExternalClassesAsync(CancellationToken cancellationToken = default)
    {
        if ((await classRepo.GetAllAsync()).Count > 0)
        {
            Console.WriteLine("Classes already exist in the database. Skipping fetch.");
            return;
        }

        var getListResponse = await client.GetAsync("https://www.dnd5eapi.co/api/2014/classes/", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<EIndexListDto>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Results.Count == 0)
        {
            Console.WriteLine("No classes found in external API.");
            return;
        }

        foreach (var item in result.Results)
        {
            var getResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/classes/{item.Index}", cancellationToken);
            var eClass = await JsonSerializer.DeserializeAsync<EClassDto>(getResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

            if (eClass is null)
            {
                Console.WriteLine($"Failed to deserialize class {item.Index}.");
                continue;
            }

            Ability? spellcastingAbility = null;

            if (eClass.Spellcasting is not null)
            {
                var abilityType = NormalizationUtil.ParseEnumOrThrow<AbilityShortType>(eClass.Spellcasting.SpellcastingAbility.Index);

                spellcastingAbility = await abilityRepo.GetByTypeAsync(abilityType)
                    ?? throw new ArgumentException($"Ability with short name {eClass.Spellcasting.SpellcastingAbility.Index} not found.");
            }

            var clss = new Class
            {
                Type = NormalizationUtil.ParseEnumOrThrow<ClassType>(eClass.Name),
                Name = eClass.Name,
                Description = "",
                HitDie = eClass.HitDie,
                ClassLevels = [],
                SpellcastingAbilityId = spellcastingAbility?.Id ?? null,
                SpellcastingAbilityType = spellcastingAbility?.Type ?? null,
                Subclasses = [],
                StartingEquipment = [],
                StartingEquipmentChoices = []
            };

            await classRepo.CreateAsync(clss);

            await AddStartingEquipmentAsync(eClass, clss, classRepo);
            await AddEquipmentChoicesAsync(eClass, clss, classRepo);
            await FetchExternalSubclassesAsync(clss, eClass.Subclasses, cancellationToken);
        }
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
                            option.AnyOfWeaponCategory = NormalizationUtil.ParseEnumOrThrow<WeaponCategory>(categoryDto.Index);
                            break;
                        default:
                            option.AnyOfWeaponType = NormalizationUtil.ParseEnumOrThrow<WeaponType>(categoryDto.Index);
                            break;
                    }

                    choice.Options.Add(option);
                }
            }

            clss.StartingEquipmentChoices.Add(choice);
        }
    }

    // Class levels based on https://www.dnd5eapi.co/api/2014/classes/{class}/levels and https://www.dnd5eapi.co/api/2014/subclasses/{subclass}/levels 
    public async Task FetchExternalClassLevelsAsync(Class clss, CancellationToken cancellationToken = default)
    {
        var getListResponse = await client.GetAsync($"https://www.dnd5eapi.co/api/2014/classes/{clss.Name}/levels", cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<List<EClassLevelDto>>(getListResponse.Content.ReadAsStream(cancellationToken), cancellationToken: cancellationToken);

        if (result is null || result.Count == 0)
        {
            Console.WriteLine("No classes found in external API.");
            return;
        }

        var abilityScoreIncreaseChoiceList = await CreateAbilityIncreaseChoiceList();
        var currentAbilityScoreBonuses = 0;

        foreach (var level in result)
        {
            var curClassLevel = new ClassLevel
            {
                Level = level.Level,
                ProficiencyBonus = level.ProficiencyBonus,
                SpellSlotsLevel1 = level.ClassSpecific.SpellSlotsLevel1,
                SpellSlotsLevel2 = level.ClassSpecific.SpellSlotsLevel2,
                SpellSlotsLevel3 = level.ClassSpecific.SpellSlotsLevel3,
                SpellSlotsLevel4 = level.ClassSpecific.SpellSlotsLevel4,
                SpellSlotsLevel5 = level.ClassSpecific.SpellSlotsLevel5,
                SpellSlotsLevel6 = level.ClassSpecific.SpellSlotsLevel6,
                SpellSlotsLevel7 = level.ClassSpecific.SpellSlotsLevel7,
                SpellSlotsLevel8 = level.ClassSpecific.SpellSlotsLevel8,
                SpellSlotsLevel9 = level.ClassSpecific.SpellSlotsLevel9,
                ActionSurges = level.ClassSpecific.ActionSurges,
                IndomitableUses = level.ClassSpecific.IndomitableUses,
                ExtraAttacks = level.ClassSpecific.ExtraAttacks
            });

            currentAbilityScoreBonuses += level.AbilityScoreBonuses;

            if (currentAbilityScoreBonuses > 0)
            {
                for (int i = 0; i < currentAbilityScoreBonuses; i++)
                {
                    curClassLevel.NewFeatures.Add(new ClassFeature
                    {
                        Name = "Ability Score Improvement",
                        Description = $"When you reach {level.Level}th level, you can increase one ability score of your choice by 2, or you can increase two ability scores of your choice by 1. As normal, you can't increase an ability score above 20 using this feature.",
                        ClassLevel = curClassLevel,
                        ClassLevelId = curClassLevel.Id,
                        AbilityIncreaseChoices = abilityScoreIncreaseChoiceList
                    });
                }
            }
        }
    }

    // Subclasses based on https://www.dnd5eapi.co/api/2014/subclasses/
    public Task FetchExternalSubclassesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task FetchExternalClassFeaturesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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
                Type = ability.Type ?? default,
                Value = 2
            };

            var choice = new AbilityIncreaseChoice
            {
                Description = $"Increase {ability.Type} by 2",
                Options = [singleAbilityValue]
            };

            abilityIncreaseChoices.Add(choice);

            var multipleAbilityValueFirst = new AbilityValue
            {
                AbilityId = ability.Id,
                Type = ability.Type ?? default,
                Value = 1
            };

            foreach (var otherAbility in abilities.Where(a => a.Id != ability.Id))
            {
                var multipleAbilityValueSecond = new AbilityValue
                {
                    AbilityId = otherAbility.Id,
                    Type = otherAbility.Type ?? default,
                    Value = 1
                };

                choice = new AbilityIncreaseChoice
                {
                    Description = $"Increase {ability.Type} by 1 and {otherAbility.Type} by 1",
                    Options = [multipleAbilityValueFirst, multipleAbilityValueSecond]
                };

                abilityIncreaseChoices.Add(choice);
            }
        }

        return abilityIncreaseChoices;
    }
}