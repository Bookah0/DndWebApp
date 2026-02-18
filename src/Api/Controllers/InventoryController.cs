using AutoMapper;
using Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Services.Interfaces.Items;
using System.ComponentModel.DataAnnotations;
using Api.Services.Interfaces;
using static Api.Validation.AllowedValues.ValuesValidator;
using Api.Validation.AllowedValues.Items;

namespace Api.Controllers.Items;

[ApiController]
[Route("api/users/{userId}/characters/{characterId}/inventory")]
public class InventoryController(IInventoryService service, ICharacterService characterService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<InventoryResponseDto>> GetInventory(int characterId)
    {
        var inventory = await service.GetByCharacterIdAsync(characterId);
        return Ok(mapper.Map<InventoryResponseDto>(inventory));
    }

    [HttpPatch("{itemId}")]
    public async Task<ActionResult<InventoryResponseDto>> AddItem(int itemId, int characterId)
    {
        var character = await characterService.GetByIdAsync(characterId);
        var updatedInventory = await service.AddItemAsync(character, itemId);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }


    [HttpDelete("{itemId}")]
    public async Task<ActionResult> RemoveItem(int itemId, int characterId)
    {
        var character = await characterService.GetByIdAsync(characterId);
        await service.DiscardItemAsync(character, itemId);
        return Ok();
    }

    [HttpGet("/equipment")]
    public async Task<ActionResult<ICollection<EquippedItemDto>>> GetEquippedItems(int characterId, [FromQuery] string? slot)
    {
        var character = await characterService.GetByIdAsync(characterId);
        return Ok(await service.GetAllEquippedItemsAsync(character, slot));
    }

    [HttpPatch("/equipment/{itemId}/{slot}")]
    public async Task<ActionResult<InventoryResponseDto>> EquipItem(int itemId, int characterId, string slot)
    {
        var character = await characterService.GetByIdAsync(characterId);
        var updatedInventory = await service.EquipAsync(character, itemId, slot);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }

    [HttpPatch("/equipment/{itemId}")]
    public async Task<ActionResult<InventoryResponseDto>> EquipItem(int itemId, int characterId)
    {
        var character = await characterService.GetByIdAsync(characterId);
        var updatedInventory = await service.EquipAsync(character, itemId);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }

    [HttpDelete("/equipment/{itemId}")]
    public async Task<ActionResult> UnequipItem(int itemId, int characterId)
    {
        var character = await characterService.GetByIdAsync(characterId);
        await service.UnEquipAsync(character, itemId);
        return Ok();
    }
}