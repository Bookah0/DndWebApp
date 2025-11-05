using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Repositories.Features;

public interface IFeatRepository : IRepository<Feat>
{

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