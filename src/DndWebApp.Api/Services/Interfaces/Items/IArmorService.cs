using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Items;

public interface IArmorService
{
    Task<Armor> CreateAsync(ArmorDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Armor>> GetAllAsync();
    Task<Armor> GetByIdAsync(int id);
    Task UpdateAsync(ArmorDto dto);
    ICollection<Armor> SortBy(ICollection<Armor> armors, ArmorSortFilter sortFilter, bool descending = false);
}