using AutoMapper;
using Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DTOs.Inventory;
using Api.Services.Interfaces.Items;
using System.ComponentModel.DataAnnotations;
using Api.Services.Interfaces;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.DTOs.Spells;
using Api.Models.Spells;
using Api.Models.Characters;

namespace Api.Controllers.Characters;

[ApiController]
[Route("api/users/{userId}/characters")]
public class CharacterController(ICharacterService service, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CharacterResponseDto>> CreateCharacter(int userId, [FromBody] CharacterDto dto)
    {
        /*
        if(userId != dto.UserId)
            throw new ValidationException($"User id in dto {dto.UserId} does not match user id in route {userId}");
        */

        var character = await service.CreateAsync(dto);
        return Ok(mapper.Map<CharacterResponseDto>(character));
    }

    [HttpGet("{characterId}")]
    public async Task<ActionResult<CharacterResponseDto>> GetCharacter(int characterId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var character =  await service.GetByIdAsync(characterId);
        return Ok(mapper.Map<CharacterResponseDto>(character));
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<CharacterResponseDto>>> GetCharacters(int userId)
    {
        var characters =  await service.GetAllByUserIdAsync(userId);
        return Ok(mapper.Map<ICollection<CharacterResponseDto>>(characters));
    }

    [HttpDelete("{characterId}")]
    public async Task<ActionResult> DeleteCharacter(int characterId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.DeleteAsync(characterId);
        return Ok();
    }

    [HttpPatch("{characterId}/levelup")]
    public async Task<ActionResult<CharacterResponseDto>> LevelUpCharacter(int characterId, int userId, [FromBody] LevelUpDto dto)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var chosenSpells = mapper.Map<ICollection<Spell>>(dto.ChosenSpells);
        var updatedCharacter = await service.LevelUpAsync(dto, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPost("{characterId}/subclass/{subclassId}")]
    public async Task<ActionResult<CharacterResponseDto>> AddSubclassToCharacter(int characterId, int subclassId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.AddSubclassAsync(subclassId, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/description")]
    public async Task<ActionResult<CharacterResponseDto>> EditCharacterDescription(int characterId, int userId, [FromBody] CharacterDescriptionDto edited)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var characterDescription = mapper.Map<CharacterDescription>(edited);
        var updatedCharacter = await service.EditCharacterDescriptionAsync(characterDescription, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/hitdice/{count}")]
    public async Task<ActionResult<CharacterResponseDto>> SpendHitDice(int count, int characterId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.SpendHitDice(count, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/longrest")]
    public async Task<ActionResult<CharacterResponseDto>> LongRest(int characterId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.LongRest(characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/takedamage/{change}")]
    
    public async Task<ActionResult<CharacterResponseDto>> TakeDamage(int characterId, int userId, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.TakeDamage(characterId, change);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/healdamage/{change}")]
    public async Task<ActionResult<CharacterResponseDto>> HealDamage(int characterId, int userId, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.HealDamage(characterId, change);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/slots/class/{slotName}/change/{change}")]
    public async Task<ActionResult<CharacterResponseDto>> EditCurrentClassSlot(int characterId, int userId, string slotName, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.EditCurrentClassSlotAsync(slotName, change, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/slots/spell/{slotLevel}/change/{change}")]
    public async Task<ActionResult<CharacterResponseDto>> EditCurrentSpellSlot(int characterId, int userId, int slotLevel, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.EditCurrentSpellSlotAsync(slotLevel, change, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    private async Task EnsureCharacterBelongsToUser(int userId, int characterId)
    {
        // TODO after implementing user service
        await Task.CompletedTask;
    }
}

public class LevelUpDto
{
    public ICollection<Spell> ChosenSpells { get; set; } = [];
}