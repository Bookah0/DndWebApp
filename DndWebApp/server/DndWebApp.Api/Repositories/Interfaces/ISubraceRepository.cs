using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ISubraceRepository : IRepository<Subrace>
{
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
    Task<Subrace?> GetWithAllDataAsync(int id);

    Task<Subrace?> GetWithTraitsAsync(int id);
    
    Task<ICollection<Subrace>> GetAllSubracesByRaceAsync(int raceId);

    // Currently unused, but may become relevant for future functionality.
    // The ParentRace can already be accessed through the Subrace’s foreign key.
    // For now, all current use cases are covered by GetAllPrimitiveDataAsync().
    Task<ICollection<Subrace>> GetAllWithAllDataAsync();
}