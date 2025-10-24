using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Backgrounds;

public interface IBackgroundRepository
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Background"/> by its <paramref name="id"/>,
    /// excluding related navigation properties:
    /// <see cref="Background.Features"/>, 
    /// <see cref="Background.StartingItems"/>, and 
    /// <see cref="Background.StartingItemsOptions"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Background"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="BackgroundDto"/> containing primitive data (Id, Name, Speed, and description strings),
    /// or <c>null</c> if no <see cref="Background"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single <see cref="Background"/>.
    /// </remarks>
    Task<BackgroundDto?> GetPrimitiveDataAsync(int id);

    /// <summary>
    /// Retrieves primitive data for all <see cref="Background"/> entities in the database,
    /// excluding related navigation properties:
    /// <see cref="Background.Features"/>, 
    /// <see cref="Background.StartingItems"/>, and 
    /// <see cref="Background.StartingItemsOptions"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="BackgroundDto"/> entities containing primitive data (Id, Name, Description and IsHowmebrew boolean).
    /// </returns>
    /// <remarks>
    /// Typically used for search results, dropdowns, or <see cref="Background"/> selection during character creation.
    /// </remarks>
    Task<ICollection<BackgroundDto>> GetAllPrimitiveDataAsync();

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