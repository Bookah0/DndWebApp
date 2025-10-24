using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Repositories.Features;

public interface IFeatRepository : IRepository<Feat>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Feat"/> by its <paramref name="id"/>,
    /// excluding related navigation properties, for example:
    /// <see cref="Feat.AbilityIncreaseChoices"/>, 
    /// <see cref="Feat.SavingThrows"/>, and 
    /// <see cref="Feat.DamageWeaknessGained"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Feat"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="FeatDto"/> containing primitive data,
    /// or <c>null</c> if no <see cref="Feat"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single <see cref="Feat"/>.
    /// </remarks>
    Task<FeatDto?> GetFeatDtoAsync(int id);

    /// <summary>
    /// Retrieves primitive data for all <see cref="Feat"/> entities in the database,
    /// excluding related navigation properties, for example:
    /// <see cref="Feat.AbilityIncreaseChoices"/>, 
    /// <see cref="Feat.SavingThrows"/>, and 
    /// <see cref="Feat.DamageWeaknessGained"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="FeatDto"/> entities containing primitive data.
    /// </returns>
    /// <remarks>
    /// Typically used for search results and dropdowns.
    /// </remarks>
    Task<ICollection<FeatDto>> GetAllFeatDtosAsync();

    /// <summary>
    /// Retrieves a <see cref="Feat"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="Feat.AbilityIncreases"/>,  
    /// <see cref="Feat.SpellsGained"/>, 
    /// <see cref="Feat.AbilityIncreaseChoices"/>, 
    /// <see cref="Feat.SkillProficiencyChoices"/>, 
    /// <see cref="Feat.ToolProficiencyChoices"/>, 
    /// <see cref="Feat.LanguageChoices"/>, 
    /// <see cref="Feat.ArmorProficiencyChoices"/>, and 
    /// <see cref="Feat.WeaponProficiencyChoices"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Feat"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="Feat"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="Feat"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for setting a <see cref="Character"/> proficiencies and apply changes to the <see cref="Character"/>s stats.
    /// </remarks>
    Task<Feat?> GetWithAllDataAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="Feat"/> entities, 
    /// including their related navigation property:
    /// <see cref="Feat.Features"/>, 
    /// <see cref="Feat.StartingItems"/>, and 
    /// <see cref="Feat.StartingItemsOptions"/> (with their related <see cref="StartingItemsOption.Choices"/>).
    /// </summary>
    /// <returns>
    /// A collection of <see cref="Feat"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete list of <see cref="Feat"/>s.
    /// </remarks>
    Task<ICollection<Feat>> GetAllWithAllDataAsync();
}