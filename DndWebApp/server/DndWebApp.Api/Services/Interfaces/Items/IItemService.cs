using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Items;

public interface IItemService
{
    Task<Item> CreateAsync(ItemDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Item>> GetAllAsync();
    Task<Item> GetByIdAsync(int id);
    Task UpdateAsync(ItemDto dto);
    ICollection<Item> SortBy(ICollection<Item> items, ItemSortFilter sortFilter, bool descending = false);
}