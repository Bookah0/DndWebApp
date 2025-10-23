using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class AbilityRepository(AppDbContext context) : EfRepository<Ability>(context)
{
    /// <summary>
    /// Retrieves primitive data from an <see cref="Ability"/> by its <paramref name="id"/>,
    /// excluding related navigation properties: <see cref="Ability.Skills"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Ability"/> to retrieve.</param>
    /// <returns>
    /// Read-only <see cref="AbilityPrimitiveDto"/> entity containing primitive data (Id, FullName, ShortName, and Description),
    /// or <c>null</c> if no ability with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typical use cases include simple display of a single ability.
    /// </remarks>
    public async Task<AbilityPrimitiveDto?> GetPrimitiveDataAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(a => new AbilityPrimitiveDto
            {
                Id = a.Id,
                FullName = a.FullName,
                ShortName = a.ShortName,
                Description = a.Description
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves an <see cref="Ability"/> entity by its <paramref name="id"/>, 
    /// including related navigation properties: <see cref="Ability.Skills"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Ability"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Ability"/> entity with all related skills,
    /// or <c>null</c> if no ability with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typical use cases include detailed display of a single ability with its related skills.
    /// </remarks>
    public async Task<Ability?> GetWithSkillsAsync(int id)
    {
        return await dbSet
            .Include(a => a.Skills)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves primitive data for all <see cref="Ability"/> entities in the database,
    /// excluding related navigation properties: <see cref="Ability.Skills"/>.
    /// </summary>
    /// <returns>
    /// Read-only <see cref="AbilityPrimitiveDto"/> entities containing primitive data (Id, FullName, ShortName, and Description).
    /// </returns>
    /// <remarks>
    /// Typical use cases include search, simple display, dropdowns, and ability selection.
    /// </remarks>
    public async Task<ICollection<AbilityPrimitiveDto>> GetAllPrimitiveDataAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(a => new AbilityPrimitiveDto
            {
                Id = a.Id,
                FullName = a.FullName,
                ShortName = a.ShortName,
                Description = a.Description
            })
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves all <see cref="Ability"/> entities, 
    /// including related navigation properties: <see cref="Ability.Skills"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="Ability"/> entities with all related skills.
    /// </returns>
    /// <remarks>
    /// Typical use cases include displaying all abilities alongside their related skills.
    /// </remarks>
    public async Task<ICollection<Ability>> GetAllWithSkillsAsync()
    {
        return await dbSet
            .Include(a => a.Skills)
            .ToListAsync();
    }
}