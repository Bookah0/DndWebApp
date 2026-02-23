using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Items.Repositories;

public interface IArmorRepository : IRepository<Armor>
{
    Task<(int, ICollection<Armor>)> GetFilteredAsync(ArmorFilterDto filter, PaginationRequestDto pagination);
}