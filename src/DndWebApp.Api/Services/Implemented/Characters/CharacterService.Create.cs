using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.World.Enums;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Services.Interfaces;

namespace DndWebApp.Api.Services.Implemented;

public partial class CharacterService : ICharacterService
{
    public async Task<Character> CreateAsync(CharacterDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.PlayerName);
        ValidationUtil.AboveZeroOrThrow(dto.Level);
        ValidationUtil.AboveZeroOrThrow(dto.RaceId);
        ValidationUtil.AboveZeroOrThrow(dto.ClassId);
        ValidationUtil.AboveZeroOrThrow(dto.BackgroundId);

        if (dto.SubraceId is not null)
            ValidationUtil.AboveZeroOrThrow(dto.SubraceId);
        if (dto.SubClassId is not null)
            ValidationUtil.AboveZeroOrThrow(dto.SubClassId);

        if (dto.CharacterDescription is not null)
        {
            if (dto.CharacterDescription.AlignmentId is not null)
                ValidationUtil.AboveZeroOrThrow(dto.CharacterDescription.AlignmentId);
            if (dto.CharacterDescription.Age is not null)
                ValidationUtil.AboveZeroOrThrow(dto.CharacterDescription.Age);
            if (dto.CharacterDescription.Height is not null)
                ValidationUtil.AboveZeroOrThrow(dto.CharacterDescription.Height);
            if (dto.CharacterDescription.Weight is not null)
                ValidationUtil.AboveZeroOrThrow(dto.CharacterDescription.Weight);
        }

        var race = await raceRepo.GetWithTraitsAsync(dto.RaceId)
            ?? throw new ArgumentException($"Race with id {dto.RaceId} could not be found");

        var clss = await classRepo.GetWithClassLevelFeaturesAsync(dto.ClassId)
            ?? throw new ArgumentException($"Class with id {dto.ClassId} could not be found");

        var background = await backgroundRepo.GetWithFeaturesAsync(dto.BackgroundId)
            ?? throw new ArgumentException($"Background with id {dto.BackgroundId} could not be found");

        var subrace = dto.SubraceId is not null ? await subraceRepo.GetWithTraitsAsync((int)dto.SubraceId!)
            ?? throw new ArgumentException($"Subrace with id {dto.SubraceId} could not be found") : null;

        var subclass = dto.SubClassId is not null ? await subclassRepo.GetWithClassLevelFeaturesAsync((int)dto.SubClassId!)
            ?? throw new ArgumentException($"Subclass with id {dto.SubClassId} could not be found") : null;

        var abilityDict = await GetAllAbilitiesAsDictionaryAsync();
        var languageDict = await GetAllLanguagesAsDictionaryAsync();
        var skillDict = await GetAllSkillsAsDictionaryAsync();

        var abilityScores = InitAbilityScoreList(dto, abilityDict);
        var dexScore = abilityScores.First(a => a.AbilityId == abilityDict[AbilityType.Dexterity].Id).Value;

        var characterStats = new CombatStats()
        {
            MaxHP = 10 + int.Parse(clss.HitDie[2..^1]) * dto.Level,
            CurrentHP = 10 + int.Parse(clss.HitDie[2..^1]) * dto.Level,
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
            ?? throw new ArgumentException($"Class level with classId {classId} at level {level} could not be found");

        return latestLevel.SpellSlots;
    }

    public ICollection<AbilityValue> InitAbilityScoreList(CharacterDto dto, Dictionary<AbilityType, Ability> repoDict)
    {
        var dtoValues = new Dictionary<AbilityType, int>
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
                Type = ValidationUtil.ParseEnumOrThrow<AbilityType>(repoDict[kvp.Key].FullName),
                //Ability = repoDict[kvp.Key],
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
                ?? throw new ArgumentException($"Class level with id {clss.Id} at level {l} could not be found");

            allFeatures.AddRange(classLevel.NewFeatures);

            if (dto.SubClassId is not null && subclass is not null)
            {
                var subclassLevel = await levelRepo.GetWithFeaturesByClassIdAsync(subclass.Id, l)
                    ?? throw new ArgumentException($"Subclass level with id {subclass.Id} at level {l} could not be found");

                allFeatures.AddRange(subclassLevel.NewFeatures);
            }
        }

        return allFeatures;
    }

    public async Task ApplyFeature(AFeature feature, int characterId)
    {
        var character = await GetByIdAsync(characterId);
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

        foreach (var abilityType in feature.SavingThrowProficiencies)
        {
            if (!abilityDict.TryGetValue(abilityType, out Ability? ability))
                throw new ArgumentException($"Ability with name {abilityType} could not be found");
            character.SavingThrows.Add(new SaveThrowProficiency { AbilityType = abilityType, AbilityId = ability.Id, FeatureId = feature.Id });
        }

        foreach (var type in feature.Languages)
        {
            if (!languageDict.TryGetValue(type, out Language? lang))
                throw new ArgumentException($"Language with name {type} could not be found");
            character.Languages.Add(new LanguageProficiency { LanguageType = type, LanguageId = lang.Id, FeatureId = feature.Id });
        }
    }

    private async Task<Dictionary<AbilityType, Ability>> GetAllAbilitiesAsDictionaryAsync()
    {
        var abilities = await abilityRepo.GetAllAsync();
        if (abilities.Count == 0)
            throw new ArgumentException("Ability list can't be empty");

        return abilities.ToDictionary(a => ValidationUtil.ParseEnumOrThrow<AbilityType>(a.FullName), a => a);
    }

    private async Task<Dictionary<LanguageType, Language>> GetAllLanguagesAsDictionaryAsync()
    {
        var languages = await languageRepo.GetAllAsync();
        if (languages.Count == 0)
            throw new ArgumentException("Language list can't be empty");

        return languages.ToDictionary(l => ValidationUtil.ParseEnumOrThrow<LanguageType>(l.Name), l => l);
    }

    private async Task<Dictionary<SkillType, Skill>> GetAllSkillsAsDictionaryAsync()
    {
        var skills = await skillRepo.GetAllAsync();
        if (skills.Count == 0)
            throw new ArgumentException("Skill list can't be empty");

        return skills.ToDictionary(s => ValidationUtil.ParseEnumOrThrow<SkillType>(s.Name), s => s);
    }
}
