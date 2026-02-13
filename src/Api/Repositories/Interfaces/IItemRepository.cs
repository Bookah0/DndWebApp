using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Models.DTOs.Items;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Repositories.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<bool> ExistsAsync(int itemId);
    Task<ICollection<Item>> GetAllMiscItemsAsync();
    Task<Item> GetByNameAsync(string name);
    Task<(int, ICollection<Item>)> GetFilteredAsync(ItemFilterDto filter, PaginationRequestDto pagination);
}