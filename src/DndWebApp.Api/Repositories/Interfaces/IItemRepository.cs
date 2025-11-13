using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    new Task<Item?> GetByIdAsync(int id);
    new Task<ICollection<Item>> GetAllAsync();
}