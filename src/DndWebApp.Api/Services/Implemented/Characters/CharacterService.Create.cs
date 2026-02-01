using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.NormalizationUtil;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters.Constants;
using DndWebApp.Api.Models.Items.Constants;

namespace DndWebApp.Api.Services.Implemented;

public partial class CharacterService : ICharacterService
{
    public async Task<Character> CreateAsync(CharacterDto dto)
    {
        var race = await raceRepo.GetWithTraitsAsync(dto.RaceId)
            ?? throw new NotFoundException($"Race with id {dto.RaceId} could not be found");

        var clss = await classRepo.GetWithClassLevelFeaturesAsync(dto.ClassId)
            ?? throw new NotFoundException($"Class with id {dto.ClassId} could not be found");

        var background = await backgroundRepo.GetWithFeaturesAsync(dto.BackgroundId)
            ?? throw new NotFoundException($"Background with id {dto.BackgroundId} could not be found");

        var subrace = dto.SubraceId is not null ? await subraceRepo.GetWithTraitsAsync((int)dto.SubraceId!)
            ?? throw new NotFoundException($"Subrace with id {dto.SubraceId} could not be found") : null;

        var subclass = dto.SubClassId is not null ? await subclassRepo.GetWithClassLevelFeaturesAsync((int)dto.SubClassId!)
            ?? throw new NotFoundException($"Subclass with id {dto.SubClassId} could not be found") : null;

        var abilityDict = await GetAllAbilitiesAsDictionaryAsync();
        var languageDict = await GetAllLanguagesAsDictionaryAsync();
        var skillDict = await GetAllSkillsAsDictionaryAsync();

        var abilityScores = InitAbilityScoreList(dto, abilityDict);
        var dexScore = abilityScores.First(a => a.AbilityId == abilityDict[AbilityType.Dexterity].Id).Value;

        var characterStats = new CombatStats()
        {
            MaxHP = 10 + clss.HitDie * dto.Level,
            CurrentHP = 10 + clss.HitDie * dto.Level,
            TempHP = 0,
            ArmorClass = 10 + dexScore / 2,
            Initiative = dexScore / 2,
            Speed = subrace is not null && subrace.Speed > race.Speed ? subrace.Speed : race.Speed,
            MaxHitDice = dto.Level,
            CurrentHitDice = dto.Level,
        };

        var inventory = new Inventory()
        {
            Currency = background.StartingCurrency,
            StoredItems = [.. background.StartingItems, .. clss.StartingEquipment],
            EquippedItems = []
        };

        var character = new Character()
        {
            Name = dto.Name,
            Level = dto.Level,
            Experience = 0,
            PlayerName = dto.PlayerName,
            TimeCreated = DateTime.UtcNow,

            Race = race,
            RaceId = dto.RaceId,
            Subrace = subrace,
            SubraceId = dto.SubraceId,

            Class = clss,
            ClassId = dto.ClassId,
            SubClass = subclass,
            SubClassId = dto.SubClassId,

            Background = background,
            BackgroundId = dto.BackgroundId,
            CharacterDescription = GetCharacterDescription(dto) ?? new(),

            Inventory = inventory,
            InventoryId = inventory.Id,

            AbilityScores = abilityScores,
            CombatStats = characterStats,
            CurrentSpellSlots = await GetSpellSlotsOfLatestLevel(dto.ClassId, dto.Level),
            ProficiencyBonus = 1 + (int)Math.Ceiling((double)dto.Level / 4),
        };

        var allFeatures = await GetAllFeaturesAsync(dto, race, subrace, background, clss, subclass);

        foreach (var feature in allFeatures)
        {
            await ApplyFeature(feature, character);
        }

        await repo.CreateAsync(character);
        return character;
    }


    public CharacterDescription? GetCharacterDescription(CharacterDto dto)
    {
        if (dto.CharacterDescription is null)
            return null;

        return new CharacterDescription()
        {
            AlignmentId = dto.CharacterDescription.AlignmentId,
            PersonalityTraits = dto.CharacterDescription.PersonalityTraits,
            Ideals = dto.CharacterDescription.Ideals,
            Bonds = dto.CharacterDescription.Bonds,
            Flaws = dto.CharacterDescription.Flaws,
            Age = dto.CharacterDescription.Age,
            Height = dto.CharacterDescription.Height,
            Weight = dto.CharacterDescription.Weight,
            Eyes = dto.CharacterDescription.Eyes,
            Skin = dto.CharacterDescription.Skin,
            Hair = dto.CharacterDescription.Hair,
            AlliesAndOrganizations = dto.CharacterDescription.AlliesAndOrganizations,
            Backstory = dto.CharacterDescription.Backstory,
            CharacterPictureUrl = dto.CharacterDescription.CharacterPictureUrl,
        };
    }

    public async Task<int[]?> GetSpellSlotsOfLatestLevel(int classId, int level)
    {
        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(classId, level)
            ?? throw new NotFoundException($"Class level with classId {classId} at level {level} could not be found");

        return latestLevel.SpellSlots;
    }

    public ICollection<AbilityValue> InitAbilityScoreList(CharacterDto dto, Dictionary<string, Ability> repoDict)
    {
        var dtoValues = new Dictionary<string, int>
        {
            { AbilityType.Strength, dto.AbilityScores.Strength },
            { AbilityType.Dexterity, dto.AbilityScores.Dexterity },
            { AbilityType.Constitution, dto.AbilityScores.Constitution },
            { AbilityType.Intelligence, dto.AbilityScores.Intelligence },
            { AbilityType.Wisdom, dto.AbilityScores.Wisdom },
            { AbilityType.Charisma, dto.AbilityScores.Charisma }
        };

        ICollection<AbilityValue> abilityScores = [.. dtoValues
            .Select(kvp => new AbilityValue
            {
                AbilityId = repoDict[kvp.Key].Id,
                Ability = repoDict[kvp.Key],
                Value = kvp.Value
            })];
        return abilityScores;
    }

    public async Task<List<AFeature>> GetAllFeaturesAsync(CharacterDto dto, Race race, Subrace? subrace, Background background, Class clss, Subclass? subclass)
    {
        List<AFeature> allFeatures = [.. race.Traits, .. background.Features];

        if (subrace is not null)
        {
            allFeatures.AddRange(subrace.Traits);
        }

        for (int l = 1; l <= dto.Level; l++)
        {
            var classLevel = await levelRepo.GetWithFeaturesByClassIdAsync(clss.Id, l)
                ?? throw new NotFoundException($"Class level with id {clss.Id} at level {l} could not be found");

            allFeatures.AddRange(classLevel.NewFeatures);

            if (dto.SubClassId is not null && subclass is not null)
            {
                var subclassLevel = await levelRepo.GetWithFeaturesByClassIdAsync(subclass.Id, l)
                    ?? throw new NotFoundException($"Subclass level with id {subclass.Id} at level {l} could not be found");

                allFeatures.AddRange(subclassLevel.NewFeatures);
            }
        }

        return allFeatures;
    }

    public async Task ApplyFeature(AFeature feature, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId) ?? throw new NotFoundException($"Character with id {characterId} could not be found");
        await ApplyFeature(feature, character);
    }

    public async Task ApplyFeature(AFeature feature, Character character)
    {
        var abilityDict = await GetAllAbilitiesAsDictionaryAsync();
        var languageDict = await GetAllLanguagesAsDictionaryAsync();

        foreach (var increase in feature.AbilityIncreases)
        {
            character.AbilityScores.First(i => i.AbilityId == increase.AbilityId).Value += increase.Value;
        }

        foreach (var spell in feature.SpellsGained)
        {
            character.ReadySpells.Add(spell);
        }

        foreach (var resistance in feature.DamageResistanceGained)
        {
            character.DamageAffinities.Add(new DamageAffinity { DamageType = resistance, AffinityType = AffinityType.Resistant, FeatureId = feature.Id });
        }

        foreach (var immunity in feature.DamageImmunityGained)
        {
            character.DamageAffinities.Add(new DamageAffinity { DamageType = immunity, AffinityType = AffinityType.Immune, FeatureId = feature.Id });
        }

        foreach (var weakness in feature.DamageWeaknessGained)
        {
            character.DamageAffinities.Add(new DamageAffinity { DamageType = weakness, AffinityType = AffinityType.Weakness, FeatureId = feature.Id });
        }

        foreach (var category in feature.WeaponCategoryProficiencies)
        {
            character.WeaponCategoryProficiencies.Add(new WeaponCategoryProficiency { WeaponCategory = category, FeatureId = feature.Id });
        }

        foreach (var type in feature.WeaponTypeProficiencies)
        {
            character.WeaponTypeProficiencies.Add(new WeaponTypeProficiency { WeaponType = type, FeatureId = feature.Id });
        }

        foreach (var type in feature.ArmorProficiencies)
        {
            character.ArmorProficiencies.Add(new ArmorProficiency { ArmorType = type, FeatureId = feature.Id });
        }

        foreach (var type in feature.ToolProficiencies)
        {
            character.ToolProficiencies.Add(new ToolProficiency { ToolType = type, FeatureId = feature.Id });
        }

        foreach (var ability in feature.SavingThrowProficiencies)
        {
            character.SavingThrows.Add(new SaveThrowProficiency { AbilityId = ability.Id, FeatureId = feature.Id });
        }

        foreach (var language in feature.Languages)
        {
            character.Languages.Add(new LanguageProficiency { LanguageId = language.Id, FeatureId = feature.Id });
        }
    }

    private async Task<Dictionary<string, Ability>> GetAllAbilitiesAsDictionaryAsync()
    {
        var abilities = await abilityRepo.GetAllAsync();
        if (abilities.Count == 0)
            throw new InvalidOperationException("Ability list can't be empty");

        return abilities.ToDictionary(a => a.FullName, a => a);
    }
    
    private async Task<Dictionary<string, Language>> GetAllLanguagesAsDictionaryAsync()
    {
        var languages = await languageRepo.GetAllAsync();
        if (languages.Count == 0)
            throw new InvalidOperationException("Language list can't be empty");

        return languages.ToDictionary(l => l.Name, l => l);
    }

    private async Task<Dictionary<string, Skill>> GetAllSkillsAsDictionaryAsync()
    {
        var skills = await skillRepo.GetAllAsync();
        if (skills.Count == 0)
            throw new InvalidOperationException("Skill list can't be empty");

        return skills.ToDictionary(s => s.Name, s => s);
    }
}
