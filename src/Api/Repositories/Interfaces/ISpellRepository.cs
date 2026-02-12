using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Spells;
using Api.Repositories.Implemented.Spells;

namespace Api.Repositories.Interfaces;

public interface ISpellRepository : IRepository<Spell>
{
    Task<Spell> GetWithClassesAsync(int id);
    Task<ICollection<Spell>> GetAllWithClassesAsync();
    Task<(int, ICollection<Spell>)> GetFilteredAsync(SpellFilterDto filter, PaginationRequestDto pagination);
}