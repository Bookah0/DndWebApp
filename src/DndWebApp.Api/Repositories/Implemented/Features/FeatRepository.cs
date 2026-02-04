using DndWebApp.Api.Data;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class FeatRepository(AppDbContext context) : IFeatureRepository<Feat>
{    
    public async Task<Feat> GetByIdAsync(int id) =>
        await context.Feats.FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new NotFoundException($"Feat with id {id} could not be found");
    
    public async Task<Feat> GetWithAllDataAsync(int id) =>
        await context.Feats
            .AsSplitQuery()
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.ArmorProficiencies)
            .Include(f => f.WeaponTypeProficiencies)
            .Include(f => f.SkillProficiencies)
            .Include(f => f.Languages)
            .Include(f => f.ToolProficiencies)
            .Include(f => f.WeaponCategoryProficiencies)
            .Include(f => f.AbilityIncreaseChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponTypeProficiencyChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.WeaponCategoryProficiencyChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Feat with id {id} could not be found");

    public async Task<Feat> GetWithProficienciesAsync(int id) =>
        await context.Feats
            .Include(f => f.AbilityIncreases)
            .Include(f => f.ArmorProficiencies)
            .Include(f => f.WeaponTypeProficiencies)
            .Include(f => f.SkillProficiencies)
            .Include(f => f.Languages)
            .Include(f => f.ToolProficiencies)
            .Include(f => f.WeaponCategoryProficiencies)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Feat with id {id} could not be found");

    public async Task<Feat> GetWithChoicesAsync(int id) =>
        await context.Feats
            .Include(f => f.AbilityIncreaseChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponTypeProficiencyChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.WeaponCategoryProficiencyChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Feat with id {id} could not be found");

    public async Task<ICollection<Feat>> GetAllAsync() => await context.Feats.ToListAsync();
    
    public async Task<Feat> CreateAsync(Feat entity)
    {
        await context.Feats.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Feat entity)
    {
        context.Feats.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Feat updatedEntity)
    {
        context.Feats.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
}