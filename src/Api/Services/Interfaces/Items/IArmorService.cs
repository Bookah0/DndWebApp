
using Api.Models.DTOs.Items;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Services.Interfaces.Items;

public interface IArmorService
{
    Task<Armor> CreateAsync(CreateArmorRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Armor>> GetAllAsync();
    Task<Armor> GetByIdAsync(int id);
    Task<Armor> UpdateAsync(UpdateArmorRequestDto dto, int id);
    Task<(int, ICollection<Armor>)> GetFilteredAsync(ArmorFilterDto filter, PaginationRequestDto pagination);
}