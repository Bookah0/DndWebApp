using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Repositories.Implemented.Spells;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ISpellRepository : IRepository<Spell>
{
    Task<Spell?> GetWithClassesAsync(int id);
    Task<ICollection<Spell>> GetAllWithClassesAsync();
    Task<ICollection<Spell>> FilterAllAsync(SpellFilter filter);
}