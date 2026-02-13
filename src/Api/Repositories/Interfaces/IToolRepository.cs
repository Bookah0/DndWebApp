using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Models.DTOs.Items;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Repositories.Interfaces;

public interface IToolRepository : IRepository<Tool>
{
    Task<Tool> GetWithAllDataAsync(int id);
    Task<(int, ICollection<Tool>)> GetFilteredAsync(ToolFilterDto filter, PaginationRequestDto pagination);
    IQueryable<Tool> SortBy(IQueryable<Tool> query, string sortFilter, bool descending = false);
}