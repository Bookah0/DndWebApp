using AutoMapper;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Services.Interfaces.Items;
using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Controllers.Items;

[ApiController]
[Route("api/users/{userId}/characters/{characterId}/inventory")]
public class InventoryController(IInventoryService service, IMapper mapper) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<InventoryResponseDto>> GetInventory(int characterId)
    {
        var inventory = await service.GetByCharacterIdAsync(characterId);
        return Ok(mapper.Map<InventoryResponseDto>(inventory));
    }

    [HttpPost("{itemId}")]
    public async Task<ActionResult<InventoryResponseDto>> AddItem(int itemId, int characterId)
    {
        var inventory = await service.GetByCharacterIdAsync(characterId);
        var updatedInventory = await service.AddItem(inventory, itemId);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }


    [HttpDelete("{itemId}")]
    public async Task<ActionResult> RemoveItem(int itemId, int characterId)
    {
        var inventory = await service.GetByCharacterIdAsync(characterId);
        await service.DiscardItem(inventory, itemId);
        return Ok();
    }

    [HttpPatch("/equipment/{itemId}/{slot}")]
    public async Task<ActionResult<InventoryResponseDto>> EquipItem(int itemId, int characterId, string slot)
    {
        var inventory = await service.GetByCharacterIdAsync(characterId);
        var updatedInventory = await service.Equip(inventory, itemId, slot);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }

    [HttpPatch("/equipment/{itemId}")]
    public async Task<ActionResult<InventoryResponseDto>> EquipItem(int itemId, int characterId)
    {
        var inventory = await service.GetByCharacterIdAsync(characterId);
        var updatedInventory = await service.Equip(inventory, itemId);
        return Ok(mapper.Map<InventoryResponseDto>(updatedInventory));
    }

    [HttpDelete("/equipment/{itemId}")]
    public async Task<ActionResult> UnequipItem(int itemId, int characterId)
    {
        var inventory = await service.GetByCharacterIdAsync(characterId);
        await service.UnEquip(inventory, itemId);
        return Ok();
    }
}