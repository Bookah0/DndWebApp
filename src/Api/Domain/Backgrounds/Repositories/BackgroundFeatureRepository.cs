using Api.Domain.Backgrounds.Models;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Repositories;
using Api.Infrastructure.Data;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Backgrounds.Repositories;

public class BackgroundFeatureRepository(AppDbContext context) : IFeatureRepository<BackgroundFeature>
{
    public async Task<BackgroundFeature> GetByIdAsync(int id) =>
        await context.BackgroundFeatures.FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");
    
    public async Task<BackgroundFeature> GetWithAllDataAsync(int id) =>
        await context.BackgroundFeatures
            .AsSplitQuery()
            .Include(b => b.Background)
            .Include(b => b.AbilityIncreases)
            .Include(b => b.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");

    public async Task<BackgroundFeature> GetWithProficienciesAsync(int id) =>
        await context.BackgroundFeatures
            .Include(f => f.AbilityIncreases)
            .Include(f => f.ArmorProficiencies)
            .Include(f => f.WeaponTypeProficiencies)
            .Include(f => f.SkillProficiencies)
            .Include(f => f.Languages)
            .Include(f => f.ToolProficiencies)
            .Include(f => f.WeaponCategoryProficiencies)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");

    public async Task<BackgroundFeature> GetWithChoicesAsync(int id) =>
        await context.BackgroundFeatures
            .Include(f => f.AbilityIncreaseChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponTypeProficiencyChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.WeaponCategoryProficiencyChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");

    public async Task<ICollection<BackgroundFeature>> GetAllAsync() => await context.BackgroundFeatures.ToListAsync();
    
    public async Task<BackgroundFeature> CreateAsync(BackgroundFeature entity)
    {
        await context.BackgroundFeatures.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    
    public async Task DeleteAsync(BackgroundFeature entity)
    {
        context.BackgroundFeatures.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<BackgroundFeature> UpdateAsync(BackgroundFeature updatedEntity)
    {
        context.BackgroundFeatures.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    private readonly Dictionary<string, IEnumerable<Func<BackgroundFeature, object>>> sortSelectorsMap = new()
    {
        { SortBackgroundFeatureOption.Name, [(f => f.Name)] },
        { SortBackgroundFeatureOption.Background, [(f => f.Background!.Name), (f => f.Name)] },
    };
}