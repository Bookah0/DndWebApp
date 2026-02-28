using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Items.Services;

public interface IWeaponService
{
    Task<Weapon> CreateAsync(CreateWeaponRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Weapon>> GetAllAsync();
    Task<Weapon> GetByIdAsync(int id);
    Task<Weapon> UpdateAsync(UpdateWeaponRequestDto dto, int id);
    Task<ICollection<Weapon>> GetAllAsync(WeaponFilterDto? filter = null, PaginationRequestDto? pagination = null);
}