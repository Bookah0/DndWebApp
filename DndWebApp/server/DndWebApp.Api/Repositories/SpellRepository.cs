using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

// TODO:
// The Spell class contains a large amount of data. To improve performance and flexibility, 
// additional getter methods should be introduced using DTOs with reduced datasets.
// More DTO variations will be added later as specific use cases and data requirements become clearer.
public class SpellRepository(AppDbContext context) : EfRepository<Spell>(context)
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
    public async Task<Spell?> GetWithClassesAsync(int id)
    {
        return await dbSet
            .Include(s => s.Classes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

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
    public async Task<ICollection<Spell>> GetAllWithClassesAsync()
    {
        return await dbSet
            .Include(s => s.Classes)
            .ToListAsync();
    }
}