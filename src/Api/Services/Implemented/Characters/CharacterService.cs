using Api.Models.Characters;
using static Api.Services.Util.SortUtil;
using static Api.Services.Util.ValidationUtil;
using static Api.Services.Util.ConstantsUtil;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Models.Spells;
using Api.Middlewares.ExceptionHandling;
using Api.Services.Constants;
using Api.Controllers.Characters;

namespace Api.Services.Implemented;

public partial class CharacterService(
    ICharacterRepository repo,
    IRaceRepository raceRepo,
    ISubraceRepository subraceRepo,
    IClassRepository classRepo,
    ISubclassRepository subclassRepo,
    IClassLevelRepository levelRepo,
    IBackgroundRepository backgroundRepo,
    IAbilityRepository abilityRepo,
    ISkillRepository skillRepo,
    ILanguageRepository languageRepo,
    ICurrentUserService currentUserService,
    ILogger<CharacterService> logger) : ICharacterService
{
    public async Task DeleteAsync(int id)
    {
        var character = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting character, Name: {CharacterName}, ID: {CharacterId}", character.Name, id);
        await repo.DeleteAsync(character);
        logger.LogInformation("Successfully deleted character, Name: {CharacterName}, ID: {CharacterId}", character.Name, id);
    }

    public async Task<ICollection<Character>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Character> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
    
    // TODO implement when user repository is ready
    public async Task<ICollection<Character>> GetAllByUserIdAsync(int userId) => await repo.GetAllAsync();

    public async Task<Character> LevelUpAsync(LevelUpDto dto, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);
        var newLvl = character.Level + 1;
        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(character.ClassId, newLvl);

        logger.LogInformation("Leveling up character, Name: {CharacterName}, ID: {CharacterId}, NewLevel: {NewLevel}", character.Name, characterId, newLvl);

        character.ProficiencyBonus = 1 + (int)Math.Ceiling((double)newLvl / 4);
        character.CurrentSpellSlots = latestLevel.SpellSlots;
        character.CurrentClassSlots = latestLevel.ClassSpecificSlotsAtLevel;

        foreach (var spell in dto.ChosenSpells)
        {
            character.ReadySpells.Add(spell);
        }

        foreach (var feature in latestLevel.NewFeatures)
        {
            await ApplyFeature(feature, characterId);
        }

        logger.LogInformation("Successfully leveled up character, Name: {CharacterName}, ID: {CharacterId}, NewLevel: {NewLevel}", character.Name, characterId, newLvl);
        return character;
    }

    public async Task<Character> AddSubclassAsync(int subclassId, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);
        var subclass = await subclassRepo.GetByIdAsync(subclassId);

        if (character.SubClassId is not null)
            throw new ValidationException($"Character already has a subclass with id {character.SubClassId}");

        logger.LogInformation("Adding subclass, CharacterName: {CharacterName}, CharacterId: {CharacterId}, SubclassId: {SubclassId}", character.Name, characterId, subclassId);
        character.SubClassId = subclassId;
        character.SubClass = subclass;
        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully added subclass, CharacterName: {CharacterName}, CharacterId: {CharacterId}, SubclassId: {SubclassId}", character.Name, characterId, subclassId);
        return character;
    }

    public async Task<Character> EditCharacterDescriptionAsync(CharacterDescription edited, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);
        logger.LogInformation("Updating character description, Name: {CharacterName}, ID: {CharacterId}", character.Name, characterId);
        character.CharacterDescription = edited;
        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully updated character description, Name: {CharacterName}, ID: {CharacterId}", character.Name, characterId);
        return character;
    }

    public async Task<Character> SpendHitDice(int nDice, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);

        if(nDice < 0)
            throw new ValidationException("Number of hit dice to spend must be a positive value.");
            
        if (character.CombatStats.CurrentHitDice - nDice < 0)
            throw new ValidationException($"Character has {character.CombatStats.CurrentHitDice} hit dice to spend, cant spend {nDice}");
        
        logger.LogInformation("Spending hit dice, Name: {CharacterName}, ID: {CharacterId}, Dice: {Dice}", character.Name, characterId, nDice);

        character.CombatStats.CurrentHitDice -= nDice;
        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully spent hit dice, Name: {CharacterName}, ID: {CharacterId}, Dice: {Dice}", character.Name, characterId, nDice);
        return character;
    }

    public async Task<Character> LongRest(int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);
        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(character.ClassId, character.Level);

        logger.LogInformation("Taking long rest, Name: {CharacterName}, ID: {CharacterId}", character.Name, characterId);
            
        character.CombatStats.CurrentHitDice = character.CombatStats.MaxHitDice;
        character.CombatStats.CurrentHP = character.CombatStats.MaxHP;
        character.CurrentClassSlots = latestLevel.ClassSpecificSlotsAtLevel;
        character.CurrentSpellSlots = latestLevel.SpellSlots;

        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully completed long rest, Name: {CharacterName}, ID: {CharacterId}", character.Name, characterId);
        return character;
    }

    public async Task<Character> TakeDamage(int characterId, int change)
    {
        if(change < 0)
            throw new ValidationException("Damage taken must be a positive value.");

        var character = await repo.GetByIdAsync(characterId);
        logger.LogInformation("Taking damage, Name: {CharacterName}, ID: {CharacterId}, Amount: {Amount}", character.Name, characterId, change);
        character.CombatStats.TempHP -= change;

        if (character.CombatStats.TempHP < 0)
        {
            character.CombatStats.CurrentHP -= character.CombatStats.TempHP;
            character.CombatStats.TempHP = 0;
        }
        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully applied damage, Name: {CharacterName}, ID: {CharacterId}, Amount: {Amount}", character.Name, characterId, change);
        return character;
    }

    public async Task<Character> HealDamage(int characterId, int change)
    {
        if(change < 0)
            throw new ValidationException("Healing amount must be a positive value.");

        var character = await repo.GetByIdAsync(characterId);
        logger.LogInformation("Healing damage, Name: {CharacterName}, ID: {CharacterId}, Amount: {Amount}", character.Name, characterId, change);
        character.CombatStats.CurrentHP += change;
        character.CombatStats.CurrentHP = Math.Min(character.CombatStats.CurrentHP, character.CombatStats.MaxHP);

        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully healed damage, Name: {CharacterName}, ID: {CharacterId}, Amount: {Amount}", character.Name, characterId, change);
        return character;
    }

    public async Task<Character> EditCurrentClassSlotAsync(string slotName, int change, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);

        if (character.CurrentSpellSlots is null)
            throw new ValidationException($"Character has no class specific slots");

        var slot = character.CurrentClassSlots.FirstOrDefault(s => s.Name == slotName) 
            ?? throw new NotFoundException($"Could not find slot with name {slotName}");

        logger.LogInformation("Updating class slot, Name: {CharacterName}, ID: {CharacterId}, Slot: {SlotName}, Change: {Change}", character.Name, characterId, slotName, change);
        slot.Quantity += change;
        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully updated class slot, Name: {CharacterName}, ID: {CharacterId}, Slot: {SlotName}, Change: {Change}", character.Name, characterId, slotName, change);
        return character;
    }
    
    public async Task<Character> EditCurrentSpellSlotAsync(int slotLevel, int change, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);

        if (character.CurrentSpellSlots is null)
            throw new ValidationException($"Character has no spellcasting");

        logger.LogInformation("Updating spell slot, Name: {CharacterName}, ID: {CharacterId}, SlotLevel: {SlotLevel}, Change: {Change}", character.Name, characterId, slotLevel, change);
        character.CurrentSpellSlots[slotLevel - 1] += change;
        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully updated spell slot, Name: {CharacterName}, ID: {CharacterId}, SlotLevel: {SlotLevel}, Change: {Change}", character.Name, characterId, slotLevel, change);
        return character;
    }

    public ICollection<Character> SortBy(ICollection<Character> characters, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortCharacterOption.AllowedValues, out string? resolved))
            return characters;

        return resolved switch
        {
            SortCharacterOption.Name => OrderByMany(characters, [(c => c.Name)], descending),
            SortCharacterOption.Level => OrderByMany(characters, [(c => c.Level), (c => c.Name)], descending),
            SortCharacterOption.TimeCreated => OrderByMany(characters, [(c => c.CreatedAt), (c => c.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }

}