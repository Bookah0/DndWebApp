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
using DndWebApp.Api.Models.Spells;

namespace DndWebApp.Api.Services.Implemented;

public partial class CharacterService : ICharacterService
{
    private readonly ICharacterRepository repo;
    private readonly IRaceRepository raceRepo;
    private readonly ISubraceRepository subraceRepo;
    private readonly IClassRepository classRepo;
    private readonly ISubclassRepository subclassRepo;
    private readonly IClassLevelRepository levelRepo;
    private readonly IBackgroundRepository backgroundRepo;
    private readonly IAbilityRepository abilityRepo;
    private readonly ISkillRepository skillRepo;
    private readonly ILanguageRepository languageRepo;

    private readonly ILogger<CharacterService> logger;

    public CharacterService(ICharacterRepository repo, IRaceRepository raceRepo, ISubraceRepository subraceRepo, IClassRepository classRepo, ISubclassRepository subclassRepo, IClassLevelRepository levelRepo, IBackgroundRepository backgroundRepo, IAbilityRepository abilityRepo, ISkillRepository skillRepo, ILanguageRepository languageRepo, ILogger<CharacterService> logger)
    {
        this.repo = repo;
        this.backgroundRepo = backgroundRepo;
        this.classRepo = classRepo;
        this.subclassRepo = subclassRepo;
        this.levelRepo = levelRepo;
        this.raceRepo = raceRepo;
        this.subraceRepo = subraceRepo;
        this.abilityRepo = abilityRepo;
        this.skillRepo = skillRepo;
        this.languageRepo = languageRepo;
        this.logger = logger;
    }

    public async Task DeleteAsync(int id)
    {
        var character = await GetByIdAsync(id);
        await repo.DeleteAsync(character);
    }

    public async Task<ICollection<Character>> GetAllAsync()
    {
        return await repo.GetMiscellaneousItemsAsync();
    }

    public async Task<Character> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new ArgumentException($"Character with id {id} could not be found");
    }

    public async Task LevelUpAsync(ICollection<Spell> chosenSpells, int characterId)
    {
        var character = await GetByIdAsync(characterId);
        var newLvl = character.Level + 1;

        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(character.ClassId, newLvl)
            ?? throw new ArgumentException($"Class level with classId {character.ClassId} at level {newLvl} could not be found");

        character.ProficiencyBonus = 1 + (int)Math.Ceiling((double)newLvl / 4);
        character.CurrentSpellSlots = latestLevel.SpellSlots;
        character.CurrentClassSlots = latestLevel.ClassSpecificSlotsAtLevel;

        foreach (var spell in chosenSpells)
        {
            character.ReadySpells.Add(spell);
        }

        foreach (var feature in latestLevel.NewFeatures)
        {
            await ApplyFeature(feature, characterId);
        }
    }

    public async Task AddSubclassAsync(int subclassId, int characterId)
    {
        var character = await GetByIdAsync(characterId);

        var subclass = await subclassRepo.GetByIdAsync(subclassId)
            ?? throw new ArgumentException($"Subclass with id {subclassId} could not be found");

        if (character.SubClassId is not null)
            throw new ArgumentException($"Character already has a subclass with id {character.SubClassId}");

        character.SubClassId = subclassId;
        character.SubClass = subclass;
        await repo.UpdateAsync(character);
    }

    public async Task EditCharacterDescriptionAsync(CharacterDescription edited, int characterId)
    {
        var character = await GetByIdAsync(characterId);
        character.CharacterDescription = edited;
        await repo.UpdateAsync(character);
    }

    public async Task SpendHitDice(int nDice, int characterId)
    {
        ValidationUtil.AboveZeroOrThrow(nDice);
        var character = await GetByIdAsync(characterId);

        if (character.CombatStats.CurrentHitDice - nDice < 0)
            throw new ArgumentException($"Character has {character.CombatStats.CurrentHitDice} hit dice to spend, cant spend {nDice}");

        character.CombatStats.CurrentHitDice -= nDice;
        await repo.UpdateAsync(character);
    }

    public async Task LongRest(int characterId)
    {
        var character = await GetByIdAsync(characterId);

        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(character.ClassId, character.Level)
            ?? throw new ArgumentException($"Class level with classId {character.ClassId} at level {character.Level} could not be found");
            
        character.CombatStats.CurrentHitDice = character.CombatStats.MaxHitDice;
        character.CombatStats.CurrentHP = character.CombatStats.MaxHP;
        character.CurrentClassSlots = latestLevel.ClassSpecificSlotsAtLevel;
        character.CurrentSpellSlots = latestLevel.SpellSlots;

        await repo.UpdateAsync(character);
    }

    public async Task TakeDamage(int characterId, int change)
    {
        ValidationUtil.AboveZeroOrThrow(change);

        var character = await GetByIdAsync(characterId);
        character.CombatStats.TempHP -= change;

        if (character.CombatStats.TempHP < 0)
        {
            character.CombatStats.CurrentHP -= character.CombatStats.TempHP;
            character.CombatStats.TempHP = 0;
        }
        await repo.UpdateAsync(character);
    }

    public async Task HealDamage(int characterId, int change)
    {
        ValidationUtil.AboveZeroOrThrow(change);

        var character = await GetByIdAsync(characterId);
        character.CombatStats.CurrentHP += change;
        character.CombatStats.CurrentHP = Math.Min(character.CombatStats.CurrentHP, character.CombatStats.MaxHP);

        await repo.UpdateAsync(character);
    }

    public async Task EditCurrentClassSlotAsync(string slotName, int change, int characterId)
    {
        var character = await GetByIdAsync(characterId);

        if (character.CurrentSpellSlots is null)
            throw new ArgumentException($"Character has no class specific slots");

        var slot = character.CurrentClassSlots.FirstOrDefault(s => s.Name == slotName) 
            ?? throw new ArgumentException($"Could not find slot with name {slotName}");

        slot.Quantity += change;
        await repo.UpdateAsync(character);
    }
    
    public async Task EditCurrentSpellSlotAsync(int slotLevel, int change, int characterId)
    {
        var character = await GetByIdAsync(characterId);

        if (character.CurrentSpellSlots is null)
            throw new ArgumentException($"Character has no spellcasting");

        character.CurrentSpellSlots[slotLevel - 1] += change;
        await repo.UpdateAsync(character);
    }

    public ICollection<Character> SortBy(ICollection<Character> characters, CharacterSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            CharacterSortFilter.Name => SortUtil.OrderByMany(characters, [(c => c.Name)], descending),
            CharacterSortFilter.Level => SortUtil.OrderByMany(characters, [(c => c.Level), (c => c.Name)], descending),
            CharacterSortFilter.TimeCreated => SortUtil.OrderByMany(characters, [(c => c.TimeCreated), (c => c.Name)], descending),
            _ => characters,
        };
    }
}