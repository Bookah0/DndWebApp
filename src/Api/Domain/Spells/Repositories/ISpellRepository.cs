using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;
using Api.Domain.Spells.DTOs;
using Api.Domain.Spells.Models;

namespace Api.Domain.Spells.Repositories;

public interface ISpellRepository : IRepository<Spell>
{
    Task<Spell> GetWithClassesAsync(int id);
    Task<ICollection<Spell>> GetAllWithClassesAsync();
    Task<(int, ICollection<Spell>)> GetFilteredAsync(SpellFilterDto filter, PaginationRequestDto pagination);
}