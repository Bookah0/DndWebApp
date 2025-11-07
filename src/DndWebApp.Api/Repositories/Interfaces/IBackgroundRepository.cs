using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IBackgroundRepository : IRepository<Background>
{
    Task<Background?> GetWithFeaturesAsync(int id);

    /// <summary>
    /// Retrieves a <see cref="Background"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="Background.Features"/>, 
    /// <see cref="Background.StartingItems"/>, and 
    /// <see cref="Background.StartingItemsOptions"/> (with their related <see cref="StartingItemsOption.Choices"/>).
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Background"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Background"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="Background"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for detailed display of a single <see cref="Background"/>, including its related navigation properties.
    /// </remarks>
    Task<Background?> GetWithAllDataAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Background"/> entities, 
    /// including their related navigation property:
    /// <see cref="Background.Features"/>, 
    /// <see cref="Background.StartingItems"/>, and 
    /// <see cref="Background.StartingItemsOptions"/> (with their related <see cref="StartingItemsOption.Choices"/>).
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Background"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete list of <see cref="Background"/>s.
    /// </remarks>
    Task<ICollection<Background>> GetAllWithAllDataAsync();
}