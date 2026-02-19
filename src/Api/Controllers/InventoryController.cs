using AutoMapper;
using Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Services.Interfaces.Items;
using System.ComponentModel.DataAnnotations;
using Api.Services.Interfaces;
using static Api.Validation.AllowedValues.ValuesValidator;
using Api.Validation.AllowedValues.Items;
using Api.Services.Util;

namespace Api.Controllers.Items;

[ApiController]
[Route("api/users/{userId}/characters/{characterId}")]
public class InventoryController(IInventoryService service, ICharacterService characterService, IMapper mapper) : ControllerBase
{
    [HttpGet("inventory")]
    public async Task<ActionResult<InventoryResponseDto>> GetInventory(int characterId)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        return Ok(mapper.Map<InventoryResponseDto>(character.Inventory));
    }

    [HttpPatch("inventory/{itemId}")]
    public async Task<ActionResult<InventoryResponseDto>> AddItem(int itemId, int characterId)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        var updatedInventory = await service.AddItemAsync(character, itemId);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }

    [HttpDelete("inventory/{itemId}")]
    public async Task<ActionResult> RemoveItem(int itemId, int characterId)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        await service.DiscardItemAsync(character, itemId);
        return Ok();
    }

    [HttpGet("equipment")]
    public async Task<ActionResult<ICollection<EquippedItemDto>>> GetAllEquippedItems(int characterId)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        return Ok(await service.GetEquippedItemsAsync(character));
    }

    [HttpGet("equipment/{slot}")]
    public async Task<ActionResult<ICollection<EquippedItemDto>>> GetItemsInSlot(int characterId, string slot)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        return Ok(await service.GetEquippedItemsAsync(character, slot));
    }

    [HttpPatch("equipment/{slot}/{itemId}")]
    public async Task<ActionResult<InventoryResponseDto>> EquipItem(int itemId, int characterId, string slot)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        var updatedInventory = await service.EquipAsync(character, itemId, slot);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }

    [HttpPatch("equipment/{itemId}")]
    public async Task<ActionResult<InventoryResponseDto>> EquipItem(int itemId, int characterId)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        var updatedInventory = await service.EquipAsync(character, itemId);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }

    [HttpDelete("equipment/{slotOrId}")]
    public async Task<ActionResult> UnequipItem(int characterId, string slotOrId)
    {
        var character = await characterService.GetWithInventoryAsync(characterId);
        
        if (int.TryParse(slotOrId, out int itemId))
            await service.UnEquipAsync(character, itemId: itemId);
        else
            await service.UnEquipAsync(character, slot: slotOrId);
        return Ok();
    }
}