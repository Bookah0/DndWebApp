using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Repositories.Backgrounds;
using DndWebApp.Api.Repositories.Characters;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Repositories.Skills;
using DndWebApp.Api.Repositories.Species;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
namespace DndWebApp.Api.Services;

public class CharacterService : IService<Character, CharacterDto>
{
    private readonly ICharacterRepository repo;
    private readonly IRaceRepository raceRepo;
    private readonly ISubraceRepository subraceRepo;
    private readonly IClassRepository classRepo;
    private readonly IClassLevelRepository levelRepo;
    private readonly IBackgroundRepository backgroundRepo;
    private readonly IAbilityRepository abilityRepo;
    private readonly ISkillRepository skillRepo;
    private readonly ILanguageRepository languageRepo;

    private readonly ILogger<CharacterService> logger;

    public CharacterService(ICharacterRepository repo, IRaceRepository raceRepo, ISubraceRepository subraceRepo, IClassRepository classRepo, IClassLevelRepository levelRepo, IBackgroundRepository backgroundRepo, IAbilityRepository abilityRepo, ISkillRepository skillRepo, ILanguageRepository languageRepo, ILogger<CharacterService> logger)
    {
        this.repo = repo;
        this.backgroundRepo = backgroundRepo;
        this.classRepo = classRepo;
        this.levelRepo = levelRepo;
        this.raceRepo = raceRepo;
        this.subraceRepo = subraceRepo;
        this.abilityRepo = abilityRepo;
        this.skillRepo = skillRepo;
        this.languageRepo = languageRepo;
        this.logger = logger;
    }

    public async Task<Character> CreateAsync(CharacterDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.PlayerName);
        ValidationUtil.AboveZeroOrThrow(dto.Level);
        ValidationUtil.AboveZeroOrThrow(dto.RaceId);
        ValidationUtil.AboveZeroOrThrow(dto.ClassId);

        if (dto.SubraceId is not null)
            ValidationUtil.AboveZeroOrThrow(dto.SubraceId);
        if (dto.SubClassId is not null)
            ValidationUtil.AboveZeroOrThrow(dto.SubClassId);
        if (dto.BackgroundId is not null)
            ValidationUtil.AboveZeroOrThrow(dto.BackgroundId);

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
            ?? throw new NullReferenceException($"Race with id {dto.RaceId} could not be found");

        var clss = await classRepo.GetWithClassLevelFeaturesAsync(dto.ClassId)
            ?? throw new NullReferenceException($"Class with id {dto.ClassId} could not be found");

        var subrace = dto.SubraceId is null ? await subraceRepo.GetWithTraitsAsync((int)dto.SubraceId)
            ?? throw new NullReferenceException($"Subrace with id {dto.SubraceId} could not be found") : null;

        var subclass = dto.SubClassId is null ? await classRepo.GetWithClassLevelFeaturesAsync((int)dto.SubClassId)
            ?? throw new NullReferenceException($"Subclass with id {dto.SubClassId} could not be found") : null;

        var background = dto.BackgroundId is null ? await backgroundRepo.GetWithFeaturesAsync((int)dto.BackgroundId)
            ?? throw new NullReferenceException($"Background with id {dto.BackgroundId} could not be found") : null;

        var allFeatures = await InitAllFeatureList(dto, race, subrace, background, clss, subclass);
        var abilityScores = await InitAbilityScoreList(dto, allFeatures);
        var allSpells = allFeatures.SelectMany(f => f.SpellsGained).ToList();
        
        var characterStats = new CombatStats()
        {
            MaxHP = 10 + int.Parse(clss.HitDie[^1].ToString()) * dto.Level, // TODO
            CurrentHP = 10 + int.Parse(clss.HitDie[^1].ToString()) * dto.Level, // TODO
            TempHP = 0,
            ArmorClass = 10 + (abilityScores.First(i => i.Ability.FullName == "Dexterity").Value / 2), // TODO
            Initiative = (abilityScores.First(i => i.Ability.FullName == "Dexterity").Value - 10) / 2, // TODO
            Speed = subrace is not null && subrace.Speed > race.Speed ? subrace.Speed : race.Speed,
            MaxHitDice = dto.Level,
            CurrentHitDice = dto.Level,
        };

        var latestClassLevel = await levelRepo.GetWithFeaturesByClassIdAsync(dto.ClassId, dto.Level)
            ?? throw new NullReferenceException($"Class level with classId {dto.ClassId} at level {dto.Level} could not be found");

        CurrentSpellSlots? currentSpellSlots = null;

        if (latestClassLevel.SpellSlotsAtLevel is not null)
        {
            currentSpellSlots = new CurrentSpellSlots()
            {
                Lvl1 = latestClassLevel.SpellSlotsAtLevel.Lvl1,
                Lvl2 = latestClassLevel.SpellSlotsAtLevel.Lvl2,
                Lvl3 = latestClassLevel.SpellSlotsAtLevel.Lvl3,
                Lvl4 = latestClassLevel.SpellSlotsAtLevel.Lvl4,
                Lvl5 = latestClassLevel.SpellSlotsAtLevel.Lvl5,
                Lvl6 = latestClassLevel.SpellSlotsAtLevel.Lvl6,
                Lvl7 = latestClassLevel.SpellSlotsAtLevel.Lvl7,
                Lvl8 = latestClassLevel.SpellSlotsAtLevel.Lvl8,
                Lvl9 = latestClassLevel.SpellSlotsAtLevel.Lvl9,
            };
        }

        var inventory = new Inventory()
        {
            Currency = null,
            EquippedItems = null
        };

        CharacterDescription characterDescription = new();

        if (dto.CharacterDescription is not null)
        {
            characterDescription = new CharacterDescription()
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

        var character = new Character()
        {
            Name = dto.Name,
            Level = dto.Level,
            Experience = 0,
            PlayerName = dto.PlayerName,
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

            CharacterDescription = characterDescription,

            Inventory = inventory,
            InventoryId = inventory.Id,

            AbilityScores = abilityScores,
            CombatStats = characterStats,
            ReadySpells = allSpells,
            CurrentSpellSlots = currentSpellSlots,

            ProficiencyBonus = 1 + (int)Math.Ceiling((double)dto.Level / 4),


        };
    }

    public async Task<List<AFeature>> InitAllFeatureList(CharacterDto dto, Race race, Subrace? subrace, Background? background, Class clss, Class? subclass)
    {
        List<AFeature> allFeatures = [.. race.Traits];

        if (subrace is not null)
        {
            allFeatures.AddRange(subrace.Traits);
        }
        if (background is not null)
        {
            allFeatures.AddRange(background.Features);
        }

        for (int l = 1; l <= dto.Level; l++)
        {
            var classLevel = await levelRepo.GetWithFeaturesByClassIdAsync(dto.ClassId, l)
                ?? throw new NullReferenceException($"Class level with classId {dto.ClassId} at level {l} could not be found");

            allFeatures.AddRange(classLevel.NewFeatures);

            if (dto.SubClassId is not null && subclass is not null)
            {
                var subclassLevel = await levelRepo.GetWithFeaturesByClassIdAsync((int)dto.SubClassId, l)
                    ?? throw new NullReferenceException($"Subclass level with subclassId {dto.SubClassId} at level {l} could not be found");

                allFeatures.AddRange(subclassLevel.NewFeatures);
            }
        }

        return allFeatures;
    }

    public async Task<List<AbilityValue>> InitAbilityScoreList(CharacterDto dto, List<AFeature> allFeatures)
    {
        var strAbility = await abilityRepo.GetByNameAsync("Strength")
            ?? throw new NullReferenceException($"Ability with name Strength could not be found");

        var dexAbility = await abilityRepo.GetByNameAsync("Dexterity")
            ?? throw new NullReferenceException($"Ability with name Dexterity could not be found");

        var conAbility = await abilityRepo.GetByNameAsync("Constitution")
            ?? throw new NullReferenceException($"Ability with name Constitution could not be found");

        var intAbility = await abilityRepo.GetByNameAsync("Intelligence")
            ?? throw new NullReferenceException($"Ability with name Intelligence could not be found");

        var wisAbility = await abilityRepo.GetByNameAsync("Wisdom")
            ?? throw new NullReferenceException($"Ability with name Wisdom could not be found");

        var chaAbility = await abilityRepo.GetByNameAsync("Charisma")
            ?? throw new NullReferenceException($"Ability with name Charisma could not be found");

        List<AbilityValue> abilityScores =
        [
            new(){ AbilityId = strAbility.Id, Ability = strAbility, Value = dto.AbilityScores.Strength },
            new(){ AbilityId = dexAbility.Id, Ability = dexAbility, Value = dto.AbilityScores.Dexterity },
            new(){ AbilityId = conAbility.Id, Ability = conAbility, Value = dto.AbilityScores.Constitution },
            new(){ AbilityId = intAbility.Id, Ability = intAbility, Value = dto.AbilityScores.Intelligence },
            new(){ AbilityId = wisAbility.Id, Ability = wisAbility, Value = dto.AbilityScores.Wisdom },
            new(){ AbilityId = chaAbility.Id, Ability = chaAbility, Value = dto.AbilityScores.Charisma }
        ];

        foreach (var feature in allFeatures)
        {
            foreach (var increase in feature.AbilityIncreases)
            {
                abilityScores.First(i => i.AbilityId == increase.AbilityId).Value += increase.Value;
            }
        }

        return abilityScores;
    }

    public async Task<ICollection<SaveThrowProficiency>> InitSaveThrowProficiencyList(List<AFeature> allFeatures)
    {
        ICollection<SaveThrowProficiency> proficiencies = [];

        foreach (var feature in allFeatures)
        {
            foreach (var abilityType in feature.SavingThrowProficiencies)
            {
                var ability = await abilityRepo.GetByNameAsync(abilityType.ToString())
                    ?? throw new NullReferenceException($"Ability with name Wisdom could not be found");

                proficiencies.Add(new() { AbilityType = abilityType, AbilityId = ability.Id, FeatureId = feature.Id });
            }
        }
        return proficiencies;
    }

    public async Task<ICollection<DamageAffinity>> InitDamageAffinityList(List<AFeature> allFeatures)
    {
        ICollection<DamageAffinity> affinities = [];

        foreach (var feature in allFeatures)
        {
            foreach (var damageType in feature.DamageResistanceGained)
            {
                affinities.Add(new() { AffinityType = AffinityType.Resistant, DamageType = damageType, FeatureId = feature.Id });
            }
            foreach (var damageType in feature.DamageImmunityGained)
            {
                affinities.Add(new() { AffinityType = AffinityType.Immune, DamageType = damageType, FeatureId = feature.Id });
            }
            foreach (var damageType in feature.DamageWeaknessGained)
            {
                affinities.Add(new() { AffinityType = AffinityType.Weakness, DamageType = damageType, FeatureId = feature.Id });
            }
        }
        return affinities;
    }

    public async Task<ICollection<SkillProficiency>> InitSkillProficiencyList(List<AFeature> allFeatures)
    {
        ICollection<SkillProficiency> proficiencies = [];

        foreach (var feature in allFeatures)
        {
            foreach (var skillType in feature.SkillProficiencies)
            {
                var skill = await skillRepo.GetByNameAsync(skillType.ToString())
                    ?? throw new NullReferenceException($"Skill with name Wisdom could not be found");

                proficiencies.Add(new() { SkillType = skillType, SkillId = skill.Id, HasExpertise = false, FeatureId = feature.Id });
            }
        }
        return proficiencies;
    }

    public async Task<ICollection<WeaponCategoryProficiency>> InitWeaponCategoryProficiencyList(List<AFeature> allFeatures)
    {
        return [.. allFeatures
            .SelectMany(f => f.WeaponCategoryProficiencies, (f, wc) => new WeaponCategoryProficiency 
            { 
                WeaponCategory = wc, 
                FeatureId = f.Id 
            })];
    }

    public async Task<ICollection<WeaponTypeProficiency>> InitWeaponTypeProficiencyList(List<AFeature> allFeatures)
    {
        return [.. allFeatures
            .SelectMany(f => f.WeaponTypeProficiencies, (f, wt) => new WeaponTypeProficiency
            {
                WeaponType = wt,
                FeatureId = f.Id
            })];
    }
    
    public async Task<ICollection<ArmorProficiency>> InitArmorProficiencyList(List<AFeature> allFeatures)
    {
        return [.. allFeatures
            .SelectMany(f => f.ArmorProficiencies, (f, at) => new ArmorProficiency 
            { 
                ArmorType = at, 
                FeatureId = f.Id 
            })];
    }

    public async Task<ICollection<ToolProficiency>> InitToolProficiencyList(List<AFeature> allFeatures)
    {
        return [.. allFeatures
            .SelectMany(f => f.ToolProficiencies, (f, tt) => new ToolProficiency
            {
                ToolType = tt,
                FeatureId = f.Id
            })];
    }

    public async Task<ICollection<LanguageProficiency>> InitLanguageProficiencyList(List<AFeature> allFeatures)
    {
        ICollection<LanguageProficiency> proficiencies = [];

        foreach (var feature in allFeatures)
        {
            foreach (var langType in feature.Languages)
            {
                var language = await languageRepo.GetByNameAsync(langType.ToString())
                    ?? throw new NullReferenceException($"Language with name {langType} could not be found");

                proficiencies.Add(new() { LanguageType = langType, LanguageId = language.Id, FeatureId = feature.Id });
            }
        }
        return proficiencies;
    }
    
    public async Task DeleteAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Ability could not be found");
        await repo.DeleteAsync(ability);
    }

    public async Task<ICollection<Character>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Character> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Ability could not be found");
    }

    public async Task UpdateAsync(CharacterDto dto)
    {

    }

    public enum CharacterSortFilter { Name, TimeCreated, Level }
    public ICollection<Character> SortBy(ICollection<Character> characters, CharacterSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            CharacterSortFilter.Name => SortUtil.OrderByMany(characters, [(l => l.Name)], descending),
            _ => characters,
        };
    }
}