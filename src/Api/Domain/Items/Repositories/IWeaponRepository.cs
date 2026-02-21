using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Items.Repositories;

public interface IWeaponRepository : IRepository<Weapon>
{
    Task<(int, ICollection<Weapon>)> GetFilteredAsync(WeaponFilterDto filter, PaginationRequestDto pagination);
    IQueryable<Weapon> SortBy(IQueryable<Weapon> query, string sortFilter, bool descending = false);
}