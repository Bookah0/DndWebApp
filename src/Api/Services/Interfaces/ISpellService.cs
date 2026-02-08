
using Api.Models.DTOs.Spells;
using Api.Models.Spells;
using Api.Services.Constants;

namespace Api.Services.Interfaces;

public interface ISpellService
{
    Task<Spell> CreateAsync(CreateSpellRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Spell>> GetAllAsync();
    Task<ICollection<Spell>> FilterAllAsync(SpellFilterDto dto);
    Task<Spell> GetByIdAsync(int id);
    Task<Spell> UpdateAsync(int id, UpdateSpellRequestDto dto);
    ICollection<Spell> SortBy(ICollection<Spell> spells, string sortFilter, bool descending = false);
}