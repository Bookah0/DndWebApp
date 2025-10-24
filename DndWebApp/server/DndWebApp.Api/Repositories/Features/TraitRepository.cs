using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Features;

public class TraitRepository(AppDbContext context) : EfRepository<Trait>(context), ITraitRepository
{
    public async Task<FeatureDto?> GetTraitDtoAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(t => new FeatureDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                IsHomebrew = t.IsHomebrew,
                FromId = t.RaceId
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<FeatureDto>> GetAllTraitDtosAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(t => new FeatureDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                IsHomebrew = t.IsHomebrew,
                FromId = t.RaceId
            })
            .ToListAsync();
    }

    public async Task<Trait?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(t => t.FromRace)
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

    public async Task<ICollection<Trait>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .AsSplitQuery()
            .Include(t => t.FromRace)
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