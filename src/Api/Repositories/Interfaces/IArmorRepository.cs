using Api.Models.DTOs.Items;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Repositories.Interfaces;

public interface IArmorRepository : IRepository<Armor>
{
    Task<(int, ICollection<Armor>)> GetFilteredAsync(ArmorFilterDto filter, PaginationRequestDto pagination);
    IQueryable<Armor> SortBy(IQueryable<Armor> query, string sortFilter, bool descending = false);
}