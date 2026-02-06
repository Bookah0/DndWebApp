
using Api.Models.DTOs.Inventory;
using Api.Models.Items;
using Api.Services.Constants;

namespace Api.Services.Interfaces.Items;

public interface IItemService
{
    Task<Item> CreateAsync(ItemDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Item>> GetAllAsync();
    Task<Item> GetByIdAsync(int id);
    Task<Item> UpdateAsync(ItemDto dto, int id);
    ICollection<Item> SortBy(ICollection<Item> items, string sortFilter, bool descending = false);
}