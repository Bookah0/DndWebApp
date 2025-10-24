using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Species;

public interface ISubraceRepository : IRepository<Subrace>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Subrace"/> by its <paramref name="id"/>,
    /// excluding related navigation properties: <see cref="Subrace.Traits"/> and <see cref="Subrace.ParentRace"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Subrace"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="SubraceDto"/> containing primitive data (Id, Name, Speed, description strings, and ParentRaceId),
    /// or <c>null</c> if no subrace with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single subrace.
    /// </remarks>
    Task<SubraceDto?> GetSubraceDtoAsync(int id);

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

    /// <summary>
    /// Retrieves primitive data for all <see cref="Subrace"/> entities in the database,
    /// excluding their <see cref="Trait"/>s and <see cref="Subrace.ParentRace"/>s.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="SubraceDto"/> entities containing primitive data (Id, Name, Speed, description strings, and ParentRaceId).
    /// </returns>
    /// <remarks>
    /// Typically used for search results, dropdowns, or subrace selection during character creation.
    /// </remarks>
    Task<ICollection<SubraceDto>> GetAllSubraceDtosAsync();

    // Currently unused, but may become relevant for future functionality.
    // The ParentRace can already be accessed through the Subrace’s foreign key.
    // For now, all current use cases are covered by GetAllPrimitiveDataAsync().
    Task<ICollection<Subrace>> GetAllWithAllDataAsync();
}