using Api.Domain.Characters.DTOs;
using Api.Domain.Characters.Services;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Domain.Spells.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Characters.Controllers;

[ApiController]
[Route("api/users/{userId}/[controller]")]
public class CharactersController(ICharacterService service, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CharacterResponseDto>> CreateCharacter(Guid userId, [FromBody] CreateCharacterRequestDto dto)
    {
        var character = await service.CreateAsync(dto);

        if(userId != character.CreatedBy)
            throw new ValidationException($"User id from character {character.CreatedBy} does not match user id in route {userId}");

        return Ok(mapper.Map<CharacterResponseDto>(character));
    }

    [HttpGet("{characterId}")]
    public async Task<ActionResult<CharacterResponseDto>> GetCharacter(int characterId, Guid userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var character =  await service.GetByIdAsync(characterId);
        return Ok(mapper.Map<CharacterResponseDto>(character));
    }

    [HttpGet("/api/characters")]
    public async Task<ActionResult<PaginationResponseDto<CharacterResponseDto>>> GetAllCharacters([FromQuery] CharacterFilterDto? filter = null, [FromQuery] PaginationRequestDto? pagination = null)
    {
        var characters =  await service.GetAllAsync(filter, pagination);
		var mappedCharacters = mapper.Map<ICollection<CharacterResponseDto>>(characters);

        return Ok(PaginationUtil.BuildPaginationResponse(mappedCharacters, pagination, $"api/characters"));
    }

    [HttpGet]
    public async Task<ActionResult<PaginationResponseDto<CharacterResponseDto>>> GetAllCharacters(Guid userId, [FromQuery] CharacterFilterDto? filter = null, [FromQuery] PaginationRequestDto? pagination = null)
    {
        var characters =  await service.GetAllAsync(filter, pagination, userId);
		var mappedCharacters = mapper.Map<ICollection<CharacterResponseDto>>(characters);

        return Ok(PaginationUtil.BuildPaginationResponse(mappedCharacters, pagination, $"api/characters"));
    }

    [HttpDelete("{characterId}")]
    public async Task<ActionResult> DeleteCharacter(int characterId, Guid userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.DeleteAsync(characterId);
        return Ok();
    }

    [HttpPatch("{characterId}/levelup")]
    public async Task<ActionResult<CharacterResponseDto>> LevelUpCharacter(int characterId, Guid userId, [FromBody] LevelUpDto dto)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var chosenSpells = mapper.Map<ICollection<Spell>>(dto.ChosenSpells);
        var updatedCharacter = await service.LevelUpAsync(dto, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPost("{characterId}/subclass/{subclassId}")]
    public async Task<ActionResult<CharacterResponseDto>> AddSubclassToCharacter(int characterId, int subclassId, Guid userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.ChangeClassAsync(subclassId, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}")]
    public async Task<ActionResult<CharacterResponseDto>> UpdateCharacter(int characterId, Guid userId, [FromBody] UpdateCharacterRequestDto request)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.UpdateAsync(request, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/hitdice/{count}")]
    public async Task<ActionResult<CharacterResponseDto>> SpendHitDice(int count, int characterId, Guid userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.SpendHitDiceAsync(count, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/longrest")]
    public async Task<ActionResult<CharacterResponseDto>> LongRest(int characterId, Guid userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.LongRestAsync(characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/takedamage/{change}")]
    
    public async Task<ActionResult<CharacterResponseDto>> TakeDamage(int characterId, Guid userId, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.TakeDamageAsync(characterId, change);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/healdamage/{change}")]
    public async Task<ActionResult<CharacterResponseDto>> HealDamage(int characterId, Guid userId, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.HealDamageAsync(characterId, change);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/slots/class/{slotName}/change/{change}")]
    public async Task<ActionResult<CharacterResponseDto>> EditCurrentClassSlot(int characterId, Guid userId, string slotName, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.EditCurrentClassSlotAsync(slotName, change, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    [HttpPatch("{characterId}/slots/spell/{slotLevel}/change/{change}")]
    public async Task<ActionResult<CharacterResponseDto>> EditCurrentSpellSlot(int characterId, Guid userId, int slotLevel, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var updatedCharacter = await service.EditCurrentSpellSlotAsync(slotLevel, change, characterId);
        return Ok(mapper.Map<CharacterResponseDto>(updatedCharacter));
    }

    private async Task EnsureCharacterBelongsToUser(Guid userId, int characterId)
    {
        var character = await service.GetByIdAsync(characterId);
        if (character.CreatedBy != userId)
            throw new ValidationException($"Character with id {characterId} does not belong to user with id {userId}"); 
    }
}

public class LevelUpDto
{
    public ICollection<Spell> ChosenSpells { get; set; } = [];
}