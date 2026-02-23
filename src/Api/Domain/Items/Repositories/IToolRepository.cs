using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Items.Repositories;

public interface IToolRepository : IRepository<Tool>
{
    Task<Tool> GetWithAllDataAsync(int id);
    Task<(int, ICollection<Tool>)> GetFilteredAsync(ToolFilterDto filter, PaginationRequestDto pagination);
}