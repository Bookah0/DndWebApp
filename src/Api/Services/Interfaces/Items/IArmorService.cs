
using Api.Models.DTOs.Inventory;
using Api.Models.Items;
using Api.Services.Constants;

namespace Api.Services.Interfaces.Items;

public interface IArmorService
{
    Task<Armor> CreateAsync(ArmorDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Armor>> GetAllAsync();
    Task<Armor> GetByIdAsync(int id);
    Task<Armor> UpdateAsync(ArmorDto dto, int id);
    ICollection<Armor> SortBy(ICollection<Armor> armors, string sortFilter, bool descending = false);
}