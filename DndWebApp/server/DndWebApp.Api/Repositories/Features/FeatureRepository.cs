using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class FeatureRepository(AppDbContext context) : EfRepository<Feature>(context), IFeatureRepository
{
    public async Task<BaseFeatureDto?> GetBaseFeatureDtoAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new BaseFeatureDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsHomebrew = r.IsHomebrew,
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<BaseFeatureDto>> GetAllBaseFeatureDtosAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new BaseFeatureDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsHomebrew = r.IsHomebrew,
            })
            .ToListAsync();
    }

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







