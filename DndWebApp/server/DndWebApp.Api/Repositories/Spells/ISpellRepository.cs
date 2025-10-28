using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;

namespace DndWebApp.Api.Repositories.Spells;

public interface ISpellRepository : IRepository<Spell>
{
    /// <summary>
    /// Retrieves a <see cref="Spell"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: <see cref="Spell.Classes"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Spell"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Spell"/> entity with its related navigation properties,
    /// or <c>null</c> if no spell with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a single spell with all related data.
    /// </remarks>
    Task<Spell?> GetWithClassesAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Spell"/> entities, 
    /// including their related navigation properties: <see cref="Spell.Classes"/>.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Spell"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying lists of spells.
    /// </remarks>
    Task<ICollection<Spell>> GetAllWithClassesAsync();

    Task<ICollection<Spell>> FilterAllAsync(SpellFilter filter);
}