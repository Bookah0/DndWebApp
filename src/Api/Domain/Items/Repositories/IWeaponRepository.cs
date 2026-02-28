using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Items.Repositories;

public interface IWeaponRepository : IRepository<Weapon>
{
    Task<ICollection<Weapon>> GetAllAsync(WeaponFilterDto? filter = null, PaginationRequestDto? pagination = null);
}