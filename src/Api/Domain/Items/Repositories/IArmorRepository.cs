using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Items.Repositories;

public interface IArmorRepository : IRepository<Armor>
{
    Task<ICollection<Armor>> GetAllAsync(ArmorFilterDto? filter = null, PaginationRequestDto? pagination = null);
}