using Api.Models.DTOs.Items;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;

namespace Api.Repositories.Interfaces;

public interface IWeaponRepository : IRepository<Weapon>
{
    Task<(int, ICollection<Weapon>)> GetFilteredAsync(WeaponFilterDto filter, PaginationRequestDto pagination);
    IQueryable<Weapon> SortBy(IQueryable<Weapon> query, string sortFilter, bool descending = false);
}