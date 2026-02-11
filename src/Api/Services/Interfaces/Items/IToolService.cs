using Api.Models.DTOs.RequestDtos.Inventory;
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
    ICollection<Tool> SortBy(ICollection<Tool> tools, string sortFilter, bool descending = false);
}