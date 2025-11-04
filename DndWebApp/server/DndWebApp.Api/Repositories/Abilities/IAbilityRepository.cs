using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Abilities;

public interface IAbilityRepository : IRepository<Ability>
{

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
    Task<Ability?> GetWithSkillsAsync(int id);

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
    Task<ICollection<Ability>> GetAllWithSkillsAsync();
}