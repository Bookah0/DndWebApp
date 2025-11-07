using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;

namespace DndWebApp.Api.Services.Interfaces.Items;
public interface IInventoryService
{
    Task<Inventory> CreateAsync(CreateInventoryDto dto);
    Task AddItem(int invId, int itemId);
    Task DiscardItem(int invId, int itemId);
    Task UnEquip(int invId, int itemId);
    Task Equip(int invId, int itemId, EquipSlot slot);
    Task<ICollection<Inventory>> GetAllAsync();
    Task<Inventory> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}