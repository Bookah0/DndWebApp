using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces.Items;

public interface IToolService
{
    Task<Tool> CreateAsync(ToolDto dto);
    Task AddProperty(ToolPropertyDto dto, int toolId);
    Task AddActivity(ToolActivityDto dto, int toolId);
    Task DeleteAsync(int id);
    Task<ICollection<Tool>> GetAllAsync();
    Task<Tool> GetByIdAsync(int id);
    Task UpdateAsync(ToolDto dto, int id);
    ICollection<Tool> SortBy(ICollection<Tool> tools, string sortFilter, bool descending = false);
}