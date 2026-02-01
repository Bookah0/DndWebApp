using AutoMapper;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Services.Interfaces.Items;
using System.ComponentModel.DataAnnotations;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.Spells;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Controllers.Characters;

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
    public async Task<ActionResult> LevelUpCharacter(int characterId, int userId, [FromBody] LevelUpDto dto)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var chosenSpells = mapper.Map<ICollection<Spell>>(dto.ChosenSpells);
        await service.LevelUpAsync(dto, characterId);
        return Ok();
    }

    [HttpPost("{characterId}/subclass/{subclassId}")]
    public async Task<ActionResult> AddSubclassToCharacter(int characterId, int subclassId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.AddSubclassAsync(subclassId, characterId);
        return Ok();
    }

    [HttpPatch("{characterId}/description")]
    public async Task<ActionResult> EditCharacterDescription(int characterId, int userId, [FromBody] CharacterDescriptionDto edited)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        var characterDescription = mapper.Map<CharacterDescription>(edited);
        await service.EditCharacterDescriptionAsync(characterDescription, characterId);
        return Ok();
    }

    [HttpPatch("{characterId}/hitdice/{count}")]
    public async Task<ActionResult> SpendHitDice(int count, int characterId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.SpendHitDice(count, characterId);
        return Ok();
    }

    [HttpPatch("{characterId}/longrest")]
    public async Task<ActionResult> LongRest(int characterId, int userId)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.LongRest(characterId);
        return Ok();
    }

    [HttpPatch("{characterId}/takedamage/{change}")]
    
    public async Task<ActionResult> TakeDamage(int characterId, int userId, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.TakeDamage(characterId, change);
        return Ok();
    }

    [HttpPatch("{characterId}/healdamage/{change}")]
    public async Task<ActionResult> HealDamage(int characterId, int userId, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.HealDamage(characterId, change);
        return Ok();
    }

    [HttpPatch("{characterId}/slots/class/{slotName}/change/{change}")]
    public async Task<ActionResult> EditCurrentClassSlot(int characterId, int userId, string slotName, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.EditCurrentClassSlotAsync(slotName, change, characterId);
        return Ok();
    }

    [HttpPatch("{characterId}/slots/spell/{slotLevel}/change/{change}")]
    public async Task<ActionResult> EditCurrentSpellSlot(int characterId, int userId, int slotLevel, int change)
    {
        await EnsureCharacterBelongsToUser(userId, characterId);
        await service.EditCurrentSpellSlotAsync(slotLevel, change, characterId);
        return Ok();
    }

    private async Task EnsureCharacterBelongsToUser(int userId, int characterId)
    {
        // TODO after implementing user service
    }
}

public class LevelUpDto
{
    public ICollection<Spell> ChosenSpells { get; set; } = [];
}