using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Features;
/*
public interface IFeatureRepository : IRepository<Feature>
{
    /// <summary>
    /// Retrieves primitive data from a <see cref="Feature"/> by its <paramref name="id"/>,
    /// excluding related navigation properties, for example:
    /// <see cref="Feature.AbilityIncreaseChoices"/>, 
    /// <see cref="Feature.SavingThrows"/>, and 
    /// <see cref="Feature.DamageWeaknessGained"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Feature"/> to retrieve.</param>
    /// <returns>
    /// A read-only <see cref="BaseFeatureDto"/> containing primitive data,
    /// or <c>null</c> if no <see cref="Feature"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// An entity of type <see cref="Feature"/> is not connected to any <see cref="Class"/>, <see cref="Background"/> or <see cref="Species"/>,
    /// therefore FromEntityId = null
    /// Typically used for simple display of a single <see cref="Feature"/>.
    /// </remarks>
    Task<BaseFeatureDto?> GetBaseFeatureDtoAsync(int id);


    /// <summary>
    /// Retrieves primitive data for all <see cref="Feature"/> entities in the database,
    /// excluding related navigation properties, for example:
    /// <see cref="Feature.AbilityIncreaseChoices"/>, 
    /// <see cref="Feature.SavingThrows"/>, and 
    /// <see cref="Feature.DamageWeaknessGained"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="BaseFeatureDto"/> entities containing primitive data.
    /// </returns>
    /// <remarks>
    /// An entity of type <see cref="Feature"/> is not connected to any <see cref="Class"/>, <see cref="Background"/> or <see cref="Species"/>,
    /// therefore FromEntityId = null
    /// Typically used for search results and dropdowns.
    /// </remarks>
    Task<ICollection<BaseFeatureDto>> GetAllBaseFeatureDtosAsync();

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
    Task<Feature?> GetWithAllDataAsync(int id);

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
    Task<ICollection<Feature>> GetAllWithAllDataAsync();
    
}
*/