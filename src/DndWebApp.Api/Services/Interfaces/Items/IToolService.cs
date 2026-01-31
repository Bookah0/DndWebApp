using DndWebApp.Api.Models.DTOs.Inventory;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces.Items;

public interface IToolService
{
    Task<Tool> CreateAsync(ToolDto dto);
    Task AddProperty(string title, string description, int toolId);
    Task AddActivity(string title, int? skillId, int? abilityId, string dc, int toolId);
    Task DeleteAsync(int id);
    Task<ICollection<Tool>> GetAllAsync();
    Task<Tool> GetByIdAsync(int id);
    Task UpdateAsync(ToolDto dto);
    ICollection<Tool> SortBy(ICollection<Tool> tools, string sortFilter, bool descending = false);
}