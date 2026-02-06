using Api.Models.DTOs.Inventory;
using Api.Models.Items;
using Api.Services.Constants;

namespace Api.Services.Interfaces.Items;

public interface IWeaponService
{
    Task<Weapon> CreateAsync(WeaponDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Weapon>> GetAllAsync();
    Task<Weapon> GetByIdAsync(int id);
    Task<Weapon> UpdateAsync(WeaponDto dto, int id);
    ICollection<Weapon> SortBy(ICollection<Weapon> weapons, string sortFilter, bool descending = false);
}