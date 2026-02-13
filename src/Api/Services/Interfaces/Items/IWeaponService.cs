using Api.Models.DTOs.Items;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Services.Interfaces.Items;

public interface IWeaponService
{
    Task<Weapon> CreateAsync(CreateWeaponRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Weapon>> GetAllAsync();
    Task<Weapon> GetByIdAsync(int id);
    Task<Weapon> UpdateAsync(UpdateWeaponRequestDto dto, int id);
    Task<(int, ICollection<Weapon>)> GetFilteredAsync(WeaponFilterDto filter, PaginationRequestDto pagination);
}