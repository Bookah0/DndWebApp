using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Classes;

public interface IClassRepository : IRepository<Class>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Class"/> by its <paramref name="id"/>,
    /// excluding related navigation properties:
    /// <see cref="Class.ClassLevels"/>, 
    /// <see cref="Class.StartingEquipment"/>, and 
    /// <see cref="Class.StartingEquipmentOptions"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Class"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="ClassDto"/> containing primitive data,
    /// or <c>null</c> if no <see cref="Class"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single <see cref="Class"/>.
    /// </remarks>
    Task<ClassDto?> GetPrimitiveDataAsync(int id);

    /// <summary>
    /// Retrieves primitive data for all <see cref="Class"/> entities in the database,
    /// excluding related navigation properties:
    /// <see cref="Class.ClassLevels"/>, 
    /// <see cref="Class.StartingEquipment"/>, and 
    /// <see cref="Class.StartingEquipmentOptions"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="ClassDto"/> entities containing primitive data.
    /// </returns>
    /// <remarks>
    /// Typically used for search results, dropdowns, or <see cref="Class"/> selection during character creation.
    /// </remarks>
    Task<ICollection<ClassDto>> GetAllPrimitiveDataAsync();

    /// <summary>
    /// Retrieves a <see cref="Class"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="Class.ClassLevels"/>, 
    /// <see cref="Class.StartingEquipment"/>, and 
    /// <see cref="Class.StartingEquipmentOptions"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Class"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Class"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="Class"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for detailed display of a single <see cref="Class"/>.
    /// </remarks>
    Task<Class?> GetWithAllDataAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Class"/> entities, 
    /// including their related navigation property:
    /// <see cref="Class.ClassLevels"/>, 
    /// <see cref="Class.StartingEquipment"/>, and 
    /// <see cref="Class.StartingEquipmentOptions"/>.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Class"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete list of <see cref="Class"/>s.
    /// </remarks>
    Task<ICollection<Class>> GetAllWithAllDataAsync();
}