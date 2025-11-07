using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.World.Enums;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Implemented;

public class CharacterService : ICharacterService
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

        var subclass = dto.SubClassId is not null ? await classRepo.GetWithClassLevelFeaturesAsync((int)dto.SubClassId!)
            ?? throw new ArgumentException($"Subclass with id {dto.SubClassId} could not be found") : null;

        var allFeatures = await GetAllFeaturesAsync(dto, race, subrace, background, clss, subclass);
        var abilityDict = await GetAllAbilitiesAsDictionaryAsync();
        var languageDict = await GetAllLanguagesAsDictionaryAsync();
        var skillDict = await GetAllSkillsAsDictionaryAsync();

        var abilityScores = InitAbilityScoreList(dto, allFeatures, abilityDict);
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

        var damageResistance = InitProficiencyList(
            allFeatures,
            f => f.DamageResistanceGained,
            (type, id) => new DamageAffinity { DamageType = type, AffinityType = AffinityType.Resistant, FeatureId = id });

        var damageImmunity = InitProficiencyList(
            allFeatures,
            f => f.DamageImmunityGained,
            (type, id) => new DamageAffinity { DamageType = type, AffinityType = AffinityType.Immune, FeatureId = id });

        var damageWeakness = InitProficiencyList(
            allFeatures,
            f => f.DamageWeaknessGained,
            (type, id) => new DamageAffinity { DamageType = type, AffinityType = AffinityType.Weakness, FeatureId = id });

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
            ReadySpells = [.. allFeatures.SelectMany(f => f.SpellsGained)],
            CurrentSpellSlots = await GetCurrentSpellSlots(dto, levelRepo),

            DamageAffinities = [.. damageResistance, .. damageImmunity, .. damageWeakness],

            WeaponCategoryProficiencies = InitProficiencyList(
                allFeatures,
                f => f.WeaponCategoryProficiencies,
                (type, id) => new WeaponCategoryProficiency { WeaponCategory = type, FeatureId = id }),

            WeaponTypeProficiencies = InitProficiencyList(
                allFeatures,
                f => f.WeaponTypeProficiencies,
                (type, id) => new WeaponTypeProficiency { WeaponType = type, FeatureId = id }),

            ArmorProficiencies = InitProficiencyList(
                allFeatures,
                f => f.ArmorProficiencies,
                (type, id) => new ArmorProficiency { ArmorType = type, FeatureId = id }),

            ToolProficiencies = InitProficiencyList(
                allFeatures,
                f => f.ToolProficiencies,
                (type, id) => new ToolProficiency { ToolType = type, FeatureId = id }),

            SkillProficiencies = InitProficiencyList(
                allFeatures,
                f => f.SkillProficiencies,
                (type, featureId) =>
                {
                    if (!skillDict.TryGetValue(type, out Skill? skill))
                        throw new ArgumentException($"Ability with name {type} could not be found");
                    return new SkillProficiency { SkillType = type, SkillId = skill.Id, HasExpertise = false, FeatureId = featureId };
                }),

            Languages = InitProficiencyList(
                allFeatures,
                f => f.Languages,
                (type, featureId) =>
                {
                    if (!languageDict.TryGetValue(type, out Language? language))
                        throw new ArgumentException($"Language with name {type} could not be found");
                    return new LanguageProficiency { LanguageType = type, LanguageId = language.Id, FeatureId = featureId };
                }),

            SavingThrows = InitProficiencyList(
                allFeatures,
                f => f.SavingThrowProficiencies,
                (type, featureId) =>
                {
                    if (!abilityDict.TryGetValue(type, out Ability? ability))
                        throw new ArgumentException($"Ability with name {type} could not be found");
                    return new SaveThrowProficiency { AbilityType = type, AbilityId = ability.Id, FeatureId = featureId };
                }),

            ProficiencyBonus = 1 + (int)Math.Ceiling((double)dto.Level / 4),
        };

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

    public async Task<CurrentSpellSlots?> GetCurrentSpellSlots(CharacterDto dto, IClassLevelRepository levelRepo)
    {
        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(dto.ClassId, dto.Level)
            ?? throw new ArgumentException($"Class level with classId {dto.ClassId} at level {dto.Level} could not be found");

        if (latestLevel.SpellSlotsAtLevel is null)
            return null;

        return new CurrentSpellSlots()
        {
            Lvl1 = latestLevel.SpellSlotsAtLevel.Lvl1,
            Lvl2 = latestLevel.SpellSlotsAtLevel.Lvl2,
            Lvl3 = latestLevel.SpellSlotsAtLevel.Lvl3,
            Lvl4 = latestLevel.SpellSlotsAtLevel.Lvl4,
            Lvl5 = latestLevel.SpellSlotsAtLevel.Lvl5,
            Lvl6 = latestLevel.SpellSlotsAtLevel.Lvl6,
            Lvl7 = latestLevel.SpellSlotsAtLevel.Lvl7,
            Lvl8 = latestLevel.SpellSlotsAtLevel.Lvl8,
            Lvl9 = latestLevel.SpellSlotsAtLevel.Lvl9,
        };
    }

    public ICollection<AbilityValue> InitAbilityScoreList(CharacterDto dto, List<AFeature> allFeatures, Dictionary<AbilityType, Ability> repoDict)
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

        List<AbilityValue> abilityScores = [.. dtoValues
            .Select(kvp => new AbilityValue
            {
                AbilityId = repoDict[kvp.Key].Id,
                Ability = repoDict[kvp.Key],
                Value = kvp.Value
            })];

        foreach (var feature in allFeatures)
        {
            foreach (var increase in feature.AbilityIncreases)
            {
                abilityScores.First(i => i.AbilityId == increase.AbilityId).Value += increase.Value;
            }
        }

        return abilityScores;
    }
    
    public async Task<List<AFeature>> GetAllFeaturesAsync(CharacterDto dto, Race race, Subrace? subrace, Background background, Class clss, Class? subclass)
    {
        List<AFeature> allFeatures = [.. race.Traits, ..background.Features];

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

    private static ICollection<T> InitProficiencyList<TSource, T>(List<AFeature> allFeatures, Func<AFeature, IEnumerable<TSource>> selector, Func<TSource, int, T> factory) where T : class
    {
        return [.. allFeatures.SelectMany(f => selector(f), (f, item) => factory(item, f.Id))];
    }

    private static async Task<ICollection<T>> InitProficiencyList<TSource, T>(List<AFeature> allFeatures, Func<AFeature, IEnumerable<TSource>> selector, Func<TSource, int, Task<T>> factory) where T : class
    {
        var tasks = allFeatures.SelectMany(f => selector(f), (f, item) => factory(item, f.Id));
        return [.. await Task.WhenAll(tasks)];
    }

    public async Task DeleteAsync(int id)
    {
        var ability = await repo.GetByIdAsync(id) ?? throw new ArgumentException("Ability could not be found");
        await repo.DeleteAsync(ability);
    }

    public async Task<ICollection<Character>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Character> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new ArgumentException($"Character with id {id} could not be found");
    }

    public async Task LevelUpAsync(int characterId)
    {

    }

    public async Task AddSubclassAsync(int subclassId, int characterId)
    {

    }

    public async Task EditCharacterDescriptionAsync(/*EditCharacterDescriptionDto dto*/)
    {

    }

    public async Task EditCombatStatsAsync(/*EditCombatStatsDto dto*/)
    {

    }

    public async Task EditCurrentSpellSlotsAsync(/*EditCurrentSpellSlotsDto dto*/)
    {

    }

    
    public ICollection<Character> SortBy(ICollection<Character> characters, CharacterSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            CharacterSortFilter.Name => SortUtil.OrderByMany(characters, [(l => l.Name)], descending),
            _ => characters,
        };
    }
}