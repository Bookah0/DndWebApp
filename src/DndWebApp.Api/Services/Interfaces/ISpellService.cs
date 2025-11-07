using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces;

public interface ISpellService
{
    Task<Spell> CreateAsync(SpellDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Spell>> GetAllAsync();
    Task<ICollection<Spell>> FilterAllAsync(SpellFilterDto dto);
    Task<Spell> GetByIdAsync(int id);
    Task UpdateAsync(SpellDto dto);
    ICollection<Spell> SortBy(ICollection<Spell> spells, SpellSortFilter sortFilter, bool descending = false);
}