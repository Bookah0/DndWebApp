using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<ICollection<Item>> GetAllMiscItemsAsync();
    Task<Item?> GetByNameAsync(string name);
}