using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IRaceRepository : IRepository<Race>
{
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

    Task<Race?> GetWithTraitsAsync(int id);
    
    // Currently unused, but may become relevant for future functionality.
    // For now, all current use cases are covered by GetAllPrimitiveDataAsync().
    Task<ICollection<Race>> GetAllWithAllDataAsync();
}