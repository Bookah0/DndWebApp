using Api.Models.DTOs.Items;
using Api.Models.DTOs.RequestDtos.Inventory;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Services.Interfaces.Items;

public interface IToolService
{
    Task<Tool> CreateAsync(CreateToolRequestDto dto);
    Task<Tool> AddProperty(ToolPropertyDto dto, int toolId);
    Task<Tool> AddActivity(ToolActivityDto dto, int toolId);
    Task DeleteAsync(int id);
    Task<ICollection<Tool>> GetAllAsync();
    Task<Tool> GetByIdAsync(int id);
    Task<Tool> UpdateAsync(UpdateToolRequestDto dto, int id);
    Task<(int, ICollection<Tool>)> GetFilteredAsync(ToolFilterDto filter, PaginationRequestDto pagination);
}