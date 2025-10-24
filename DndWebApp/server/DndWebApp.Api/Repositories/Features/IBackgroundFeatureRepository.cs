using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Features;

public interface IBackgroundFeatureRepository : IRepository<BackgroundFeature>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="BackgroundFeature"/> by its <paramref name="id"/>,
    /// excluding related navigation properties, for example:
    /// <see cref="BackgroundFeature.AbilityIncreaseChoices"/>, 
    /// <see cref="BackgroundFeature.SavingThrows"/>, and 
    /// <see cref="BackgroundFeature.DamageWeaknessGained"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="BackgroundFeature"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="FeatureDto"/> containing primitive data,
    /// or <c>null</c> if no <see cref="BackgroundFeature"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single <see cref="BackgroundFeature"/>.
    /// </remarks>
    Task<FeatureDto?> GetBackgroundDtoAsync(int id);

    /// <summary>
    /// Retrieves primitive data for all <see cref="BackgroundFeature"/> entities in the database,
    /// excluding related navigation properties, for example:
    /// <see cref="BackgroundFeature.AbilityIncreaseChoices"/>, 
    /// <see cref="BackgroundFeature.SavingThrows"/>, and 
    /// <see cref="BackgroundFeature.DamageWeaknessGained"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="FeatureDto"/> entities containing primitive data.
    /// </returns>
    /// <remarks>
    /// Typically used for search results and dropdowns.
    /// </remarks>
    Task<ICollection<FeatureDto>> GetAllBackgroundDtosAsync();

    /// <summary>
    /// Retrieves a <see cref="BackgroundFeature"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="BackgroundFeature.AbilityIncreases"/>,  
    /// <see cref="BackgroundFeature.SpellsGained"/>, 
    /// <see cref="BackgroundFeature.AbilityIncreaseChoices"/>, 
    /// <see cref="BackgroundFeature.SkillProficiencyChoices"/>, 
    /// <see cref="BackgroundFeature.ToolProficiencyChoices"/>, 
    /// <see cref="BackgroundFeature.LanguageChoices"/>, 
    /// <see cref="BackgroundFeature.ArmorProficiencyChoices"/>, and 
    /// <see cref="BackgroundFeature.WeaponProficiencyChoices"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="BackgroundFeature"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="BackgroundFeature"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="BackgroundFeature"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for setting a <see cref="Character"/> proficiencies and apply changes to the <see cref="Character"/>s stats.
    /// </remarks>
    Task<BackgroundFeature?> GetWithAllDataAsync(int id);

    /// <summary>
    /// Retrieves all <see cref="BackgroundFeature"/> entities, 
    /// including their related navigation property:
    /// <see cref="BackgroundFeature.Features"/>, 
    /// <see cref="BackgroundFeature.StartingItems"/>, and 
    /// <see cref="BackgroundFeature.StartingItemsOptions"/> (with their related <see cref="StartingItemsOption.Choices"/>).
    /// </summary>
    /// <returns>
    /// A collection of <see cref="BackgroundFeature"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete list of <see cref="BackgroundFeature"/>s.
    /// </remarks>
    Task<ICollection<BackgroundFeature>> GetAllWithAllDataAsync();
}