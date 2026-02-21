using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Items.Services;

public interface IArmorService
{
    Task<Armor> CreateAsync(CreateArmorRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Armor>> GetAllAsync();
    Task<Armor> GetByIdAsync(int id);
    Task<Armor> UpdateAsync(UpdateArmorRequestDto dto, int id);
    Task<(int, ICollection<Armor>)> GetFilteredAsync(ArmorFilterDto filter, PaginationRequestDto pagination);
}