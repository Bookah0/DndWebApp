using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

// TODO:
// FeatureRepository classes contain large amounts of duplicated code.
public class FeatureRepository(AppDbContext context) : EfRepository<Feature>(context)
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
    /// A read-only <see cref="BaseFeaturePrimitiveDto"/> containing primitive data,
    /// or <c>null</c> if no <see cref="Feature"/> with the specified <paramref name="id"/> exists.
    /// </returns>
    /// <remarks>
    /// An entity of type <see cref="Feature"/> is not connected to any <see cref="Class"/>, <see cref="Background"/> or <see cref="Species"/>,
    /// therefore FromEntityId = null
    /// Typically used for simple display of a single <see cref="Feature"/>.
    /// </remarks>
    public async Task<BaseFeaturePrimitiveDto?> GetPrimitiveDataAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new BaseFeaturePrimitiveDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsHomebrew = r.IsHomebrew,
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves primitive data for all <see cref="Feature"/> entities in the database,
    /// excluding related navigation properties, for example:
    /// <see cref="Feature.AbilityIncreaseChoices"/>, 
    /// <see cref="Feature.SavingThrows"/>, and 
    /// <see cref="Feature.DamageWeaknessGained"/>.
    /// </summary>
    /// <returns>
    /// A collection of read-only <see cref="BaseFeaturePrimitiveDto"/> entities containing primitive data.
    /// </returns>
    /// <remarks>
    /// An entity of type <see cref="Feature"/> is not connected to any <see cref="Class"/>, <see cref="Background"/> or <see cref="Species"/>,
    /// therefore FromEntityId = null
    /// Typically used for search results and dropdowns.
    /// </remarks>
    public async Task<ICollection<BaseFeaturePrimitiveDto>> GetAllPrimitiveDataAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new BaseFeaturePrimitiveDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsHomebrew = r.IsHomebrew,
            })
            .ToListAsync();
    }

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
    public async Task<Feature?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .Where(f => !(f is ClassFeature) && !(f is Feat) && !(f is BackgroundFeature) && !(f is Trait))
            .AsSplitQuery()
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .Include(f => f.LanguageChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponProficiencyChoices)
            .Include(f => f.AbilityIncreaseChoices)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }


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
    public async Task<ICollection<Feature>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .Where(f => !(f is ClassFeature) && !(f is Feat) && !(f is BackgroundFeature) && !(f is Trait))
            .AsSplitQuery()
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .Include(f => f.LanguageChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponProficiencyChoices)
            .Include(f => f.AbilityIncreaseChoices)
                .ThenInclude(o => o.Options)
            .ToListAsync();
    }
}







