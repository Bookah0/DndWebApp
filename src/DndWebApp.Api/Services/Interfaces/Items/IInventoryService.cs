
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Services.Interfaces.Items;

public interface IInventoryService
{
    Task<Inventory> CreateAsync(CreateInventoryDto dto);
    Task<Inventory> AddItem(Inventory inventory, int itemId);
    Task DiscardItem(Inventory inventory, int itemId);
    Task<Inventory> Equip(Inventory inventory, int itemId, string slot);
    Task<Inventory> Equip(Inventory inventory, int itemId);
    Task UnEquip(Inventory inventory, int itemId);
    Task UnEquip(Inventory inventory, string slot);
    Task<Inventory> GetByCharacterIdAsync(int characterId);
}