using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Items.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Task<bool> ExistsAsync(int itemId);
    Task<ICollection<Item>> GetAllMiscItemsAsync();
    Task<Item> GetByNameAsync(string name);
    Task<ICollection<Item>> GetAllAsync(ItemFilterDto? filter = null, PaginationRequestDto? pagination = null);
}