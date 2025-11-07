using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IToolRepository : IRepository<Tool>
{
    /// <summary>
    /// Retrieves an <see cref="Tool"/> entity by its <paramref name="id"/>, 
    /// including related navigation properties: <see cref="Tool.Properties"/> and <see cref="Tool.Activities"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Tool"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Tool"/> entity with all related activities and properties,
    /// or <c>null</c> if no ability with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typical use cases include detailed display of a single <see cref="Tool"/> with its activities and properties.
    /// </remarks>
    Task<Tool?> GetWithAllDataAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Tool"/> entities, 
    /// including related navigation properties: <see cref="Tool.Properties"/> and <see cref="Tool.Activities"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="Tool"/> entities with all related activities and properties.
    /// </returns>
    /// <remarks>
    /// Typical use cases include displaying all <see cref="Tool"/>s alongside their related activities and properties.
    /// </remarks>
    Task<ICollection<Tool>> GetAllWithAllDataAsync();
}