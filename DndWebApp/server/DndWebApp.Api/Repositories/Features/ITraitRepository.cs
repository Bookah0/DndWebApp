using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Features;

public interface ITraitRepository : IRepository<Trait>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Trait"/> by its <paramref name="id"/>,
    /// excluding related navigation properties, for example:
    /// <see cref="Trait.AbilityIncreaseChoices"/>, 
    /// <see cref="Trait.SavingThrows"/>, and 
    /// <see cref="Trait.DamageWeaknessGained"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Trait"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="TraitDto"/> containing primitive data,
    /// or <c>null</c> if no <see cref="Trait"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single <see cref="Trait"/>.
    /// </remarks>
    Task<TraitDto?> GetDtoAsync(int id);


    /// <summary>
    /// Retrieves primitive data for all <see cref="Trait"/> entities in the database,
    /// excluding related navigation properties, for example:
    /// <see cref="Trait.AbilityIncreaseChoices"/>, 
    /// <see cref="Trait.SavingThrows"/>, and 
    /// <see cref="Trait.DamageWeaknessGained"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="TraitDto"/> entities containing primitive data.
    /// </returns>
    /// <remarks>
    /// Typically used for search results and dropdowns.
    /// </remarks>
    Task<ICollection<TraitDto>> GetAllDtosAsync();

    /// <summary>
    /// Retrieves a <see cref="Trait"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="Trait.AbilityIncreases"/>,  
    /// <see cref="Trait.SpellsGained"/>, 
    /// <see cref="Trait.AbilityIncreaseChoices"/>, 
    /// <see cref="Trait.SkillProficiencyChoices"/>, 
    /// <see cref="Trait.ToolProficiencyChoices"/>, 
    /// <see cref="Trait.LanguageChoices"/>, 
    /// <see cref="Trait.ArmorProficiencyChoices"/>, and 
    /// <see cref="Trait.WeaponProficiencyChoices"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Trait"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Trait"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="Trait"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for setting a <see cref="Character"/> proficiencies and apply changes to the <see cref="Character"/>s stats.
    /// </remarks>
    Task<Trait?> GetWithAllDataAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Trait"/> entities, 
    /// including their related navigation property:
    /// <see cref="Trait.Features"/>, 
    /// <see cref="Trait.StartingItems"/>, and 
    /// <see cref="Trait.StartingItemsOptions"/> (with their related <see cref="StartingItemsOption.Choices"/>).
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Trait"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete list of <see cref="Trait"/>s.
    /// </remarks>
    Task<ICollection<Trait>> GetAllWithAllDataAsync();

    
    
    
    
}