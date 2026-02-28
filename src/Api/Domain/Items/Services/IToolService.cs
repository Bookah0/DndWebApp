using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Items.Services;

public interface IToolService
{
    Task<Tool> CreateAsync(CreateToolRequestDto dto);
    Task<Tool> AddProperty(ToolPropertyDto dto, int toolId);
    Task<Tool> AddActivity(ToolActivityDto dto, int toolId);
    Task DeleteAsync(int id);
    Task<ICollection<Tool>> GetAllAsync();
    Task<Tool> GetByIdAsync(int id);
    Task<Tool> UpdateAsync(UpdateToolRequestDto dto, int id);
    Task<ICollection<Tool>> GetAllAsync(ToolFilterDto? filter = null, PaginationRequestDto? pagination = null);
}