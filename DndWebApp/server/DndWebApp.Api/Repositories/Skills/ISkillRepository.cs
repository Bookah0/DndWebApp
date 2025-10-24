using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Skills;

public interface ISkillRepository : IRepository<Skill>
{
    /// <summary>
    /// Retrieves a <see cref="Skill"/> entity by its <paramref name="id"/>, 
    /// including its related navigation property: <see cref="Skill.Ability"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Skill"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Skill"/> entity with its related ability,
    /// or <c>null</c> if no skill with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying detailed information for a single skill.
    /// </remarks>
    Task<Skill?> GetWithAbilityAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Skill"/> entities, 
    /// including their related navigation property: <see cref="Skill.Ability"/>.
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Skill"/> entities with their related ability.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete list of skills.
    /// </remarks>
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
}