using Api.Domain.Characters.DTOs;
using Api.Domain.Characters.Models;
using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;

namespace Api.Domain.Characters.Services;

public interface IInventoryService
{
    Task<Inventory> CreateAsync(CreateInventoryDto dto);
    Task<Inventory> GetByCharacterIdAsync(int characterId);

    Task<Inventory> AddItemAsync(int characterId, int itemId, int quantity = 1) ;
    Task<Inventory> AddItemAsync(Character character, int itemId, int quantity = 1);
    Task<Inventory> DiscardItemAsync(int characterId, int itemId, int quantity = 1) ;
    Task<Inventory> DiscardItemAsync(Character character, int itemId, int quantity = 1);

    Task<ICollection<Item>> GetStoredItemsAsync(Inventory inventory);

    Task<ICollection<EquippedItemDto>> GetEquippedItemsAsync(Character character, string? slot = null);
    Task UnEquipAsync(Character character, int? itemId = null, string? slot = null);
    Task<Inventory> EquipAsync(Character character, int itemId, string? slot = null);
}