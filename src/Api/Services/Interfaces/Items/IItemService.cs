
using Api.Models.DTOs.Items;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Services.Interfaces.Items;

public interface IItemService
{
    Task<Item> CreateAsync(CreateItemRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Item>> GetAllAsync();
    Task<Item> GetByIdAsync(int id);
    Task<Item> UpdateAsync(UpdateItemRequestDto dto, int id);
    Task<(int, ICollection<Item>)> GetFilteredAsync(ItemFilterDto filter, PaginationRequestDto pagination);
}