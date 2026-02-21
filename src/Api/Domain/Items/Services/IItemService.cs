using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Items.Services;

public interface IItemService
{
    Task<Item> CreateAsync(CreateItemRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Item>> GetAllAsync();
    Task<Item> GetByIdAsync(int id);
    Task<Item> UpdateAsync(UpdateItemRequestDto dto, int id);
    Task<(int, ICollection<Item>)> GetFilteredAsync(ItemFilterDto filter, PaginationRequestDto pagination);
}