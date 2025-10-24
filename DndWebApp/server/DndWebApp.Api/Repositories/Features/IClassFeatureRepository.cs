using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Features;

public interface IClassFeatureRepository : IRepository<ClassFeature>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="ClassFeature"/> by its <paramref name="id"/>,
    /// excluding related navigation properties, for example:
    /// <see cref="ClassFeature.AbilityIncreaseChoices"/>, 
    /// <see cref="ClassFeature.SavingThrows"/>, and 
    /// <see cref="ClassFeature.DamageWeaknessGained"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="ClassFeature"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="FeatureDto"/> containing primitive data,
    /// or <c>null</c> if no <see cref="ClassFeature"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for simple display of a single <see cref="ClassFeature"/>.
    /// </remarks>
    Task<FeatureDto?> GetClassFeatureDtoAsync(int id);

    /// <summary>
    /// Retrieves primitive data for all <see cref="ClassFeature"/> entities in the database,
    /// excluding related navigation properties, for example:
    /// <see cref="ClassFeature.AbilityIncreaseChoices"/>, 
    /// <see cref="ClassFeature.SavingThrows"/>, and 
    /// <see cref="ClassFeature.DamageWeaknessGained"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="FeatureDto"/> entities containing primitive data.
    /// </returns>
    /// <remarks>
    /// Typically used for search results and dropdowns.
    /// </remarks>
    Task<ICollection<FeatureDto>> GetAllClassFeaturDtosAsync();

    /// <summary>
    /// Retrieves a <see cref="ClassFeature"/> entity by its <paramref name="id"/>, 
    /// including its related navigation properties: 
    /// <see cref="ClassFeature.AbilityIncreases"/>,  
    /// <see cref="ClassFeature.SpellsGained"/>, 
    /// <see cref="ClassFeature.AbilityIncreaseChoices"/>, 
    /// <see cref="ClassFeature.SkillProficiencyChoices"/>, 
    /// <see cref="ClassFeature.ToolProficiencyChoices"/>, 
    /// <see cref="ClassFeature.LanguageChoices"/>, 
    /// <see cref="ClassFeature.ArmorProficiencyChoices"/>, and 
    /// <see cref="ClassFeature.WeaponProficiencyChoices"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="ClassFeature"/> to retrieve.</param>
    /// <returns>
    /// The <see cref="ClassFeature"/> entity with its related navigation properties,
    /// or <c>null</c> if no <see cref="ClassFeature"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// Typically used for setting a <see cref="Character"/> proficiencies and apply changes to the <see cref="Character"/>s stats.
    /// </remarks>
    Task<ClassFeature?> GetWithAllDataAsync(int id);
    
    /// <summary>
    /// Retrieves all <see cref="ClassFeature"/> entities, 
    /// including their related navigation property:
    /// <see cref="ClassFeature.Features"/>, 
    /// <see cref="ClassFeature.StartingItems"/>, and 
    /// <see cref="ClassFeature.StartingItemsOptions"/> (with their related <see cref="StartingItemsOption.Choices"/>).
    /// </summary>
    /// <returns>
    /// A collection of <see cref="ClassFeature"/> entities with their related navigation properties.
    /// </returns>
    /// <remarks>
    /// Typically used when displaying a complete list of <see cref="ClassFeature"/>s.
    /// </remarks>
    Task<ICollection<ClassFeature>> GetAllWithAllDataAsync();
}