using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class ClassFeatureRepository(AppDbContext context) : EfRepository<ClassFeature>(context), IClassFeatureRepository
{
    public async Task<FeatureDto?> GetClassFeatureDtoAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(f => new FeatureDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                IsHomebrew = f.IsHomebrew,
                FromId = f.ClassId
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<FeatureDto>> GetAllClassFeaturDtosAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(f => new FeatureDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                IsHomebrew = f.IsHomebrew,
                FromId = f.ClassId
            })
            .ToListAsync();
    }

    public async Task<ClassFeature?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(f => f.Class)
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

    public async Task<ICollection<ClassFeature>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .AsSplitQuery()
            .Include(f => f.Class)
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