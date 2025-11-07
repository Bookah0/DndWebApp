using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ICharacterRepository : IRepository<Character>
{
    /// <summary>
    /// Retrieves data representing the current spell slots from a <see cref="Character"/> by its <paramref name="id"/>,
    /// excluding related navigation properties.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Character"/>.</param>
    /// <returns>
    /// A read-only <see cref="CharacterSpellSlotsDto"/> containing spellslot data,
    /// or <c>null</c> if no <see cref="Character"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for displaying <see cref="Character"/>s current spellslots.
    /// </remarks>
    Task<CurrentSpellSlots?> GetCurrentSpellSlotsAsync(int id);

    /// <summary>
    /// Retrieves data representing the description of a <see cref="Character"/> by its <paramref name="id"/>,
    /// excluding related navigation properties.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Character"/>.</param>
    /// <returns>
    /// A read-only <see cref="CharacterDescriptionDto"/> containing data such as eye color, ideals and background story,
    /// or <c>null</c> if no <see cref="Character"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for displaying detailed <see cref="Character"/> description.
    /// </remarks>
    Task<CharacterDescriptionDto?> GetCharacterDescriptionAsync(int id);

    /// <summary>
    /// Retrieves a <see cref="Feature"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="Feature.AbilityIncreases"/>,  
    /// <see cref="Feature.SpellsGained"/>, 
    /// <see cref="Feature.AbilityIncreaseChoices"/>, 
    /// <see cref="Feature.SkillProficiencyChoices"/>, 
    /// <see cref="Feature.ToolProficiencyChoices"/>, 
    /// <see cref="Feature.LanguageChoices"/>, 
    /// <see cref="Feature.ArmorProficiencyChoices"/>, and 
    /// <see cref="Feature.WeaponProficiencyChoices"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Feature"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Feature"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="Feature"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for setting a <see cref="Character"/> proficiencies and apply changes to the <see cref="Character"/>s stats.
    /// </remarks>
    Task<Character?> GetWithAllDataAsync(int id);
}