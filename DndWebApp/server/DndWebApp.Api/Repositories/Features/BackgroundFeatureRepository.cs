using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class BackgroundFeatureRepository(AppDbContext context) : EfRepository<BackgroundFeature>(context), IBackgroundFeatureRepository
{
    public async Task<BackgroundFeatureDto?> GetDtoAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(b => new BackgroundFeatureDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                IsHomebrew = b.IsHomebrew,
                BackgroundId = b.BackgroundId
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<BackgroundFeatureDto>> GetAllDtosAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(b => new BackgroundFeatureDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                IsHomebrew = b.IsHomebrew,
                BackgroundId = b.BackgroundId
            })
            .ToListAsync();
    }

    public async Task<BackgroundFeature?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.Background)
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

    public async Task<ICollection<BackgroundFeature>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.Background)
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