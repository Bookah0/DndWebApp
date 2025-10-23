using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class ClassLevelRepository(AppDbContext context) : EfRepository<ClassLevel>(context)
{
    /// <summary>
    /// Retrieves a <see cref="ClassLevel"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="ClassLevel.SpellSlotsAtLevel"/>, 
    /// <see cref="ClassLevel.ClassSpecificSlotsAtLevel"/>, and 
    /// <see cref="ClassLevel.NewFeatures"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="ClassLevel"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="ClassLevel"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="ClassLevel"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for detailed display of a single <see cref="ClassLevel"/> in the level up menu.
    /// </remarks>
    public async Task<ClassLevel?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.SpellSlotsAtLevel)
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }


    /// <summary>
    /// Retrieves all <see cref="ClassLevel"/> entities, 
    /// including their related navigation property:
    /// <see cref="ClassLevel.SpellSlotsAtLevel"/>, 
    /// <see cref="ClassLevel.ClassSpecificSlotsAtLevel"/>, and 
    /// <see cref="ClassLevel.NewFeatures"/>.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="ClassLevel"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete <see cref="Class"/>s and all its <see cref="ClassLevel"/>s.
    /// </remarks>
    public async Task<ICollection<ClassLevel>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.SpellSlotsAtLevel)
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .ToListAsync();
    }
}