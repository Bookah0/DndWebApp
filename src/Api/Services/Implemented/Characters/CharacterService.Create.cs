using Api.Models.Characters;
using Api.Models.Features;
using Api.Models.Items;
using Api.Services.Interfaces;
using Api.Middlewares.ExceptionHandling;
using Api.Models.Characters.Constants;
using Api.Models.Items.Constants;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Services.Util;

namespace Api.Services.Implemented;

public partial class CharacterService : ICharacterService
{
    public async Task<Character> CreateAsync(CreateCharacterRequestDto dto)
    {
        var race = await raceRepo.GetWithTraitsAsync(dto.RaceId);
        var clss = await classRepo.GetWithClassLevelFeaturesAsync(dto.ClassId);
        var background = await backgroundRepo.GetWithFeaturesAsync(dto.BackgroundId);

        var subrace = dto.SubraceId is not null 
            ? await subraceRepo.GetWithTraitsAsync((int)dto.SubraceId!) 
            : null;

        var subclass = dto.SubClassId is not null
            ? await subclassRepo.GetWithClassLevelFeaturesAsync((int)dto.SubClassId!) 
            : null;

        logger.LogInformation("Creating character, Name: {CharacterName}, ClassId: {ClassId}, RaceId: {RaceId}", dto.Name, dto.ClassId, dto.RaceId);

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
            PlayerName = dto.PlayerName ?? "",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),

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

        var allFeatures = await GetAllFeaturesAsync(character, race, subrace, background, clss, subclass);

        foreach (var feature in allFeatures)
        {
            await ApplyFeatureAsync(feature, character);
        }

        var createdCharacter = await repo.CreateAsync(character);
        logger.LogInformation("Successfully created character, Name: {CharacterName}, ID: {CharacterId}", createdCharacter.Name, createdCharacter.Id);
        return createdCharacter;
    }


    public CharacterDescription? GetCharacterDescription(CreateCharacterRequestDto dto)
    {
        if (dto.CharacterDescription is null)
            return null;

        return new CharacterDescription()
        {
            AlignmentId = dto.CharacterDescription.AlignmentId,
            PersonalityTraits = dto.CharacterDescription.PersonalityTraits ?? "",
            Ideals = dto.CharacterDescription.Ideals ?? "",
            Bonds = dto.CharacterDescription.Bonds ?? "",
            Flaws = dto.CharacterDescription.Flaws ?? "",
            Age = dto.CharacterDescription.Age,
            Height = dto.CharacterDescription.Height,
            Weight = dto.CharacterDescription.Weight,
            Eyes = dto.CharacterDescription.Eyes ?? "",
            Skin = dto.CharacterDescription.Skin ?? "",
            Hair = dto.CharacterDescription.Hair ?? "",
            AlliesAndOrganizations = dto.CharacterDescription.AlliesAndOrganizations ?? "",
            Backstory = dto.CharacterDescription.Backstory ?? "",
            CharacterPictureUrl = dto.CharacterDescription.CharacterPictureUrl ?? "",
        };
    }

    public async Task<int[]?> GetSpellSlotsOfLatestLevel(int classId, int level)
    {
        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(classId, level);
        return latestLevel.SpellSlots;
    }

    public ICollection<AbilityValue> InitAbilityScoreList(CreateCharacterRequestDto dto, Dictionary<string, Ability> repoDict)
    {
        var dtoValues = new Dictionary<string, int>
        {
            { AbilityType.Strength, dto.AbilityScores.Str },
            { AbilityType.Dexterity, dto.AbilityScores.Dex },
            { AbilityType.Constitution, dto.AbilityScores.Con },
            { AbilityType.Intelligence, dto.AbilityScores.Int },
            { AbilityType.Wisdom, dto.AbilityScores.Wis },
            { AbilityType.Charisma, dto.AbilityScores.Cha }
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

    public async Task<List<Feature>> GetAllFeaturesAsync(Character character, Race race, Subrace? subrace, Background background, BaseClass clss, Subclass? subclass)
    {
        List<Feature> allFeatures = [.. race.Traits, .. background.Features];

        if (subrace is not null)
        {
            allFeatures.AddRange(subrace.Traits);
        }

        for (int l = 1; l <= character.Level; l++)
        {
            var classLevel = await levelRepo.GetWithFeaturesByClassIdAsync(clss.Id, l);
            allFeatures.AddRange(classLevel.NewFeatures);

            if (character.SubClassId is not null && subclass is not null)
            {
                var subclassLevel = await levelRepo.GetWithFeaturesByClassIdAsync(subclass.Id, l);
                allFeatures.AddRange(subclassLevel.NewFeatures);
            }
        }

        return allFeatures;
    }

    public async Task ApplyFeatureAsync(Feature feature, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);
        await ApplyFeatureAsync(feature, character);
    }

    public async Task ApplyFeatureAsync(Feature feature, Character character)
    {
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

    public async Task RemoveFeatureAsync(Feature feature, Character character)
    {
        character.ReadySpells.RemoveMany(feature.SpellsGained);
        
        foreach (var increase in feature.AbilityIncreases)
        {
            character.AbilityScores.First(i => i.AbilityId == increase.AbilityId).Value -= increase.Value;
        }

        foreach (var resistance in feature.DamageResistanceGained)
        {
            character.DamageAffinities.RemoveFirst(da => da.DamageType == resistance && da.AffinityType == AffinityType.Resistant && da.FeatureId == feature.Id);
        }

        foreach (var immunity in feature.DamageImmunityGained)
        {
            character.DamageAffinities.RemoveFirst(da => da.DamageType == immunity && da.AffinityType == AffinityType.Immune && da.FeatureId == feature.Id);
        }

        foreach (var weakness in feature.DamageWeaknessGained)
        {
            character.DamageAffinities.RemoveFirst(da => da.DamageType == weakness && da.AffinityType == AffinityType.Weakness && da.FeatureId == feature.Id);
        }

        foreach (var category in feature.WeaponCategoryProficiencies)
        {
            character.WeaponCategoryProficiencies.RemoveFirst(wcp => wcp.WeaponCategory == category && wcp.FeatureId == feature.Id);
        }

        foreach (var type in feature.WeaponTypeProficiencies)
        {
            character.WeaponTypeProficiencies.RemoveFirst(wtp => wtp.WeaponType == type && wtp.FeatureId == feature.Id);
        }

        foreach (var type in feature.ArmorProficiencies)
        {
            character.ArmorProficiencies.RemoveFirst(ap => ap.ArmorType == type && ap.FeatureId == feature.Id);
        }

        foreach (var type in feature.ToolProficiencies)
        {
            character.ToolProficiencies.RemoveFirst(tp => tp.ToolType == type && tp.FeatureId == feature.Id);
        }

        foreach (var ability in feature.SavingThrowProficiencies)
        {
            character.SavingThrows.RemoveFirst(st => st.AbilityId == ability.Id && st.FeatureId == feature.Id);
        }

        foreach (var language in feature.Languages)
        {
            character.Languages.RemoveFirst(lp => lp.LanguageId == language.Id && lp.FeatureId == feature.Id);
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
