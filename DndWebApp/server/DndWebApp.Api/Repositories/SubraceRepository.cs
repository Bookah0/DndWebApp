using System.IO.Compression;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class SubraceRepository(AppDbContext context) : EfRepository<Subrace>(context)
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Subrace"/> by its <paramref name="id"/>,
    /// excluding related navigation properties: <see cref="Subrace.Traits"/> and <see cref="Subrace.ParentRace"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Subrace"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="SubracePrimitiveDto"/> containing primitive data (Id, Name, Speed, description strings, and ParentRaceId),
    /// or <c>null</c> if no subrace with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single subrace.
    /// </remarks>
    public async Task<SubracePrimitiveDto?> GetPrimitiveDataAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new SubracePrimitiveDto
            {
                Id = r.Id,
                Name = r.Name,
                GeneralDescription = r.RaceDescription.GeneralDescription,
                ParentRaceId = r.ParentRaceId,
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves a <see cref="Subrace"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: <see cref="Subrace.Traits"/> and <see cref="Subrace.ParentRace"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Subrace"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Subrace"/> entity with its related traits and parent race,
    /// or <c>null</c> if no subrace with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for detailed display of a single subrace, including its traits and parent race.
    /// </remarks>
    public async Task<Subrace?> GetWithAllDataAsync(int id)
    {
        return await dbSet
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves primitive data for all <see cref="Subrace"/> entities in the database,
    /// excluding their <see cref="Trait"/>s and <see cref="Subrace.ParentRace"/>s.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="SubracePrimitiveDto"/> entities containing primitive data (Id, Name, Speed, description strings, and ParentRaceId).
    /// </returns>
    /// <remarks>
    /// Typically used for search results, dropdowns, or subrace selection during character creation.
    /// </remarks>
    public async Task<ICollection<SubracePrimitiveDto>> GetAllPrimitiveDataAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new SubracePrimitiveDto
            {
                Id = r.Id,
                Name = r.Name,
                GeneralDescription = r.RaceDescription.GeneralDescription,
                ParentRaceId = r.ParentRaceId,
            })
            .ToListAsync();
    }

    // Currently unused, but may become relevant for future functionality.
    // The ParentRace can already be accessed through the Subrace’s foreign key.
    // For now, all current use cases are covered by GetAllPrimitiveDataAsync().
    public async Task<ICollection<Subrace>> GetAllWithAllDataAsync()
    {
        return await dbSet
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .ToListAsync();
    }
}