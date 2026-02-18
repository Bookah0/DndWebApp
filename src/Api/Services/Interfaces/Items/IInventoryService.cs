
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Services.Interfaces.Items;

public interface IInventoryService
{
    Task<Inventory> CreateAsync(CreateInventoryDto dto);
    Task<Inventory> GetByCharacterIdAsync(int characterId);

    Task<Inventory> AddItemAsync(int characterId, int itemId, int quantity = 1) ;
    Task<Inventory> AddItemAsync(Character character, int itemId, int quantity = 1);
    Task<Inventory> DiscardItemAsync(int characterId, int itemId, int quantity = 1) ;
    Task<Inventory> DiscardItemAsync(Character character, int itemId, int quantity = 1);
    
    Task<ICollection<EquippedItemDto>> GetAllEquippedItemsAsync(Character character, string? slot);
    Task UnEquipAsync(Character character, int itemId);
    Task UnEquipAsync(Character character, string slot);
    Task<Inventory> EquipAsync(Character character, int itemId, string slot);
    Task<Inventory> EquipAsync(Character character, int itemId);
}