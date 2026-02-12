
using Api.Models.DTOs.ResponseDtos;
using Api.Models.DTOs.Spells;
using Api.Models.Spells;
using Api.Repositories.Implemented.Spells;

namespace Api.Services.Interfaces;

public interface ISpellService
{
    Task<Spell> CreateAsync(CreateSpellRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Spell>> GetAllAsync();
    Task<(int, ICollection<Spell>)> GetFilteredAsync(SpellFilterDto filter, PaginationRequestDto pagination);
    Task<Spell> GetByIdAsync(int id);
    Task<Spell> UpdateAsync(int id, UpdateSpellRequestDto dto);
}