using Api.Domain.Abilities.Repositories;
using Api.Domain.Backgrounds.Repositories;
using Api.Domain.Characters.Controllers;
using Api.Domain.Characters.DTOs;
using Api.Domain.Characters.Models;
using Api.Domain.Characters.Repositories;
using Api.Domain.Classes.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Species.Repositories;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Infrastructure.Validation;
using static Api.Domain.Shared.Utils.QueryExtensions;

namespace Api.Domain.Characters.Services;

public partial class CharacterService(
    ICharacterRepository repo,
    IRaceRepository raceRepo,
    ISubraceRepository subraceRepo,
    IBaseClassRepository classRepo,
    ISubclassRepository subclassRepo,
    IClassLevelRepository levelRepo,
    IBackgroundRepository backgroundRepo,
    IAbilityRepository abilityRepo,
    IInventoryService inventoryService,
    ICurrentUserService currentUserService,
    ILogger<CharacterService> logger) : ICharacterService
{
	public async Task<ICollection<Character>> GetAllAsync(CharacterFilterDto? filter = null, PaginationRequestDto? pagination = null, Guid? userId = null)
	{
		if(userId is not null && userId != currentUserService.GetCurrentUserId())
			throw new ValidationException("Logged in user id does not match the requested user id.");
		
		return await repo.GetAllAsync(filter, pagination, userId);
	}

    public async Task<ICollection<Character>> GetAllAsync() => await repo.GetAllAsync();
	public async Task<Character> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
    public async Task<Character> GetWithInventoryAsync(int id) => await repo.GetWithInventoryAsync(id);
    public async Task<ICollection<Character>> GetAllByUserIdAsync(Guid userId)
    {
        var allCharacters = await repo.GetAllAsync();
        return [.. allCharacters.Where(c => c.CreatedBy == userId)];
    }
    public async Task<ICollection<Character>> GetAllByCurrentUserAsync() => await GetAllByUserIdAsync(currentUserService.GetCurrentUserId());

    public async Task<Character> UpdateAsync(UpdateCharacterRequestDto dto, int id)
    {
        var character = await repo.GetByIdAsync(id);
        logger.LogInformation("Updating character, Name: {CharacterName}, ID: {CharacterId}", character.Name, id);

        character.Name = dto.Name ?? character.Name;
        character.PlayerName = dto.PlayerName ?? character.PlayerName;
        character.Experience = dto.Experience ?? character.Experience;
        
        character.Info.AlignmentId = dto.CharacterInfo.AlignmentId ?? character.Info.AlignmentId;
        character.Info.Age = dto.CharacterInfo.Age ?? character.Info.Age;
        character.Info.Height = dto.CharacterInfo.Height ?? character.Info.Height;
        character.Info.Weight = dto.CharacterInfo.Weight ?? character.Info.Weight;
        character.Info.Hair = dto.CharacterInfo.Hair ?? character.Info.Hair;
        character.Info.Eyes = dto.CharacterInfo.Eyes ?? character.Info.Eyes;
        character.Info.Skin = dto.CharacterInfo.Skin ?? character.Info.Skin;
        character.Info.PersonalityTraits = dto.CharacterInfo.PersonalityTraits ?? character.Info.PersonalityTraits;
        character.Info.Ideals = dto.CharacterInfo.Ideals ?? character.Info.Ideals;
        character.Info.Bonds = dto.CharacterInfo.Bonds ?? character.Info.Bonds;
        character.Info.Flaws = dto.CharacterInfo.Flaws ?? character.Info.Flaws;
        character.Info.AlliesAndOrganizations = dto.CharacterInfo.AlliesAndOrganizations ?? character.Info.AlliesAndOrganizations;
        character.Info.Backstory = dto.CharacterInfo.Backstory ?? character.Info.Backstory;
        character.Info.CharacterPictureUrl = dto.CharacterInfo.CharacterPictureUrl ?? character.Info.CharacterPictureUrl;

        character.IsPublic = dto.IsPublic ?? character.IsPublic;
        character.CloningAllowed = dto.CloningAllowed ?? character.CloningAllowed;
        character = await repo.UpdateAsync(character);

        logger.LogInformation("Successfully updated character, Name: {CharacterName}, ID: {CharacterId}", character.Name, id);
        return character;
    }

    public async Task DeleteAsync(int id)
    {
        var character = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting character, Name: {CharacterName}, ID: {CharacterId}", character.Name, id);
        await repo.DeleteAsync(character);
        logger.LogInformation("Successfully deleted character, Name: {CharacterName}, ID: {CharacterId}", character.Name, id);
    }

    public async Task<Character> LevelUpAsync(LevelUpDto dto, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);
        var newLvl = character.Level + 1;
        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(character.ClassId, newLvl);

        logger.LogInformation("Leveling up character, Name: {CharacterName}, ID: {CharacterId}, NewLevel: {NewLevel}", character.Name, characterId, newLvl);

        character.ProficiencyBonus = 1 + (int)Math.Ceiling((double)newLvl / 4);
        character.CurrentSpellSlots = latestLevel.SpellSlots;
        character.CurrentClassSlots = latestLevel.ClassSlotsAtLevel;

        foreach (var spell in dto.ChosenSpells)
        {
            character.ReadySpells.Add(spell);
        }

        foreach (var feature in latestLevel.NewFeatures)
        {
            ApplyFeature(feature, character);
        }

        logger.LogInformation("Successfully leveled up character, Name: {CharacterName}, ID: {CharacterId}, NewLevel: {NewLevel}", character.Name, characterId, newLvl);
        return character;
    }

    public async Task<Character> ChangeSubclassAsync(int newSubclassId, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);

        if (character.SubClassId is not null && character.SubClassId == newSubclassId)
            throw new ValidationException($"Character already has a subclass with id {character.SubClassId}");

        if(character.SubClassId is not null)
        {
            var subclass = await subclassRepo.GetWithLevelFeaturesAsync((int)character.SubClassId);

            foreach (var feature in subclass.ClassLevels.SelectMany(cl => cl.NewFeatures))
            {
                RemoveFeature(feature, character);
            }
        }
        
        var newSubclass = await subclassRepo.GetWithLevelFeaturesAsync(newSubclassId);

        logger.LogInformation("Changing subclass, CharacterName: {CharacterName}, CharacterId: {CharacterId}, SubclassId: {SubclassId}", character.Name, characterId, newSubclassId);
        character.SubClassId = newSubclassId;
        character.SubClass = newSubclass;

        foreach (var feature in newSubclass.ClassLevels.SelectMany(cl => cl.NewFeatures))
        {
            ApplyFeature(feature, character);
        }


        await repo.UpdateAsync(character);
        logger.LogInformation("Successfully changed subclass, CharacterName: {CharacterName}, CharacterId: {CharacterId}, SubclassId: {SubclassId}", character.Name, characterId, newSubclassId);
        return character;
    }

    public async Task<Character> ChangeClassAsync(int characterId, int newClassId)
    {
        var character = await repo.GetWithClassesAsync(characterId);
        var newClass = await classRepo.GetByIdAsync(newClassId);
        
        if (character.SubClassId is not null)
            throw new ValidationException($"Character already has a subclass with id {character.SubClassId}");

        logger.LogInformation("Changing class, CharacterName: {CharacterName}, CharacterId: {CharacterId}, NewClassId: {NewClassId}", character.Name, characterId, newClassId);
        character.ClassId = newClassId;
        character.Class = newClass;
        character = await repo.UpdateAsync(character);
        logger.LogInformation("Successfully changed class, CharacterName: {CharacterName}, CharacterId: {CharacterId}, NewClassId: {NewClassId}", character.Name, characterId, newClassId);
        return character;
    }

    public async Task<Character> SpendHitDiceAsync(int nDice, int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);

        if(nDice < 0)
            throw new ValidationException("Number of hit dice to spend must be a positive value.");
            
        if (character.CombatStats.CurrentHitDice - nDice < 0)
            throw new ValidationException($"Character has {character.CombatStats.CurrentHitDice} hit dice to spend, cant spend {nDice}");
        
        logger.LogInformation("Spending hit dice, Name: {CharacterName}, ID: {CharacterId}, Dice: {Dice}", character.Name, characterId, nDice);
        character.CombatStats.CurrentHitDice -= nDice;
        character = await repo.UpdateAsync(character);
        logger.LogInformation("Successfully spent hit dice, Name: {CharacterName}, ID: {CharacterId}, Dice: {Dice}", character.Name, characterId, nDice);
        return character;
    }

    public async Task<Character> LongRestAsync(int characterId)
    {
        var character = await repo.GetByIdAsync(characterId);
        var latestLevel = await levelRepo.GetWithFeaturesByClassIdAsync(character.ClassId, character.Level);

        logger.LogInformation("Taking long rest, Name: {CharacterName}, ID: {CharacterId}", character.Name, characterId);
            
        character.CombatStats.CurrentHitDice = character.CombatStats.MaxHitDice;
        character.CombatStats.CurrentHP = character.CombatStats.MaxHP;
        character.CurrentClassSlots = latestLevel.ClassSlotsAtLevel;
        character.CurrentSpellSlots = latestLevel.SpellSlots;

        character = await repo.UpdateAsync(character);
        logger.LogInformation("Successfully completed long rest, Name: {CharacterName}, ID: {CharacterId}", character.Name, characterId);
        return character;
    }

    public async Task<Character> TakeDamageAsync(int characterId, int change)
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
        character = await repo.UpdateAsync(character);
        logger.LogInformation("Successfully applied damage, Name: {CharacterName}, ID: {CharacterId}, Amount: {Amount}", character.Name, characterId, change);
        return character;
    }

    public async Task<Character> HealDamageAsync(int characterId, int change)
    {
        if(change < 0)
            throw new ValidationException("Healing amount must be a positive value.");

        var character = await repo.GetByIdAsync(characterId);
        logger.LogInformation("Healing damage, Name: {CharacterName}, ID: {CharacterId}, Amount: {Amount}", character.Name, characterId, change);
        character.CombatStats.CurrentHP += change;
        character.CombatStats.CurrentHP = Math.Min(character.CombatStats.CurrentHP, character.CombatStats.MaxHP);

        character = await repo.UpdateAsync(character);
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
        character = await repo.UpdateAsync(character);
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
        character = await repo.UpdateAsync(character);
        logger.LogInformation("Successfully updated spell slot, Name: {CharacterName}, ID: {CharacterId}, SlotLevel: {SlotLevel}, Change: {Change}", character.Name, characterId, slotLevel, change);
        return character;
    }
}