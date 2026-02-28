using Api.Domain.Shared.DTOs;
using Api.Domain.Spells.DTOs;
using Api.Domain.Spells.Models;

namespace Api.Domain.Spells.Services;

public interface ISpellService
{
    Task<Spell> CreateAsync(CreateSpellRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Spell>> GetAllAsync();
    Task<ICollection<Spell>> GetAllAsync(SpellFilterDto? filter = null, PaginationRequestDto? pagination = null);
    Task<Spell> GetByIdAsync(int id);
    Task<Spell> UpdateAsync(int id, UpdateSpellRequestDto dto);
}