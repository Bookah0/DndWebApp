using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Items;

public interface IWeaponService
{
    Task<Weapon> CreateAsync(WeaponDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Weapon>> GetAllAsync();
    Task<Weapon> GetByIdAsync(int id);
    Task UpdateAsync(WeaponDto dto);
    ICollection<Weapon> SortBy(ICollection<Weapon> weapons, WeaponSortFilter sortFilter, bool descending = false);
}