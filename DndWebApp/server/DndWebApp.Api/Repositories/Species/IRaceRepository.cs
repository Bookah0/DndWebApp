using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Species;

public interface IRaceRepository : IRepository<Race>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Race"/> by its <paramref name="id"/>,
    /// excluding related navigation properties: <see cref="Race.Traits"/> and <see cref="Race.SubRaces"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Race"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="RacePrimitiveDto"/> containing primitive data (Id, Name, Speed, and description strings),
    /// or <c>null</c> if no race with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single race.
    /// </remarks>
    Task<RacePrimitiveDto?> GetPrimitiveDataAsync(int id);

    /// <summary>
    /// Retrieves a <see cref="Race"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: <see cref="Race.Traits"/> and <see cref="Race.SubRaces"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Race"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Race"/> entity with its related traits and subraces,
    /// or <c>null</c> if no race with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for detailed display of a single race, including its traits and subraces.
    /// </remarks>
    Task<Race?> GetWithAllDataAsync(int id);

    /// <summary>
    /// Retrieves primitive data for all <see cref="Race"/> entities in the database,
    /// excluding their <see cref="Trait"/>s and <see cref="Subrace"/>s.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="RacePrimitiveDto"/> entities containing primitive data (Id, Name, Speed, and description strings).
    /// </returns>
    /// <remarks>
    /// Typically used for search results, dropdowns, or race selection during character creation.
    /// </remarks>
    Task<ICollection<RacePrimitiveDto>> GetAllPrimitiveDataAsync();

    // Currently unused, but may become relevant for future functionality.
    // For now, all current use cases are covered by GetAllPrimitiveDataAsync().
    Task<ICollection<Race>> GetAllWithAllDataAsync();
}