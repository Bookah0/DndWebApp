
using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces.Items;

public interface IArmorService
{
    Task<Armor> CreateAsync(ArmorDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Armor>> GetAllAsync();
    Task<Armor> GetByIdAsync(int id);
    Task UpdateAsync(ArmorDto dto, int id);
    ICollection<Armor> SortBy(ICollection<Armor> armors, string sortFilter, bool descending = false);
}