
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.Items;

namespace Api.Services.Interfaces.Items;

public interface IArmorService
{
    Task<Armor> CreateAsync(CreateArmorRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Armor>> GetAllAsync();
    Task<Armor> GetByIdAsync(int id);
    Task<Armor> UpdateAsync(UpdateArmorRequestDto dto, int id);
    ICollection<Armor> SortBy(ICollection<Armor> armors, string sortFilter, bool descending = false);
}