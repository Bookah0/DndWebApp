using DndWebApp.Api.Data;
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class ClassFeatureRepository : IFeatureRepository<ClassFeature>
{
    private readonly AppDbContext context;

    public ClassFeatureRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<ClassFeature> CreateAsync(ClassFeature entity)
    {
        await context.ClassFeatures.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<ClassFeature>> GetAllAsync() => await context.ClassFeatures.ToListAsync();
    public async Task<ClassFeature?> GetByIdAsync(int id) => await context.ClassFeatures.FirstOrDefaultAsync(f => f.Id == id);

    public async Task DeleteAsync(ClassFeature entity)
    {
        context.ClassFeatures.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ClassFeature updatedEntity)
    {
        context.ClassFeatures.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ClassFeature?> GetWithAllDataAsync(int id)
    {
        return await context.ClassFeatures
            .AsSplitQuery()
            .Include(f => f.Level)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassFeature>> GetAllWithAllDataAsync()
    {
        return await context.ClassFeatures
            .AsSplitQuery()
            .Include(f => f.Level)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .ToListAsync();
    }

        public async Task<ClassFeature> GetWithProficienciesAsync(int id)
    {
        return await context.ClassFeatures
            .Include(f => f.AbilityIncreases)
            .Include(f => f.ArmorProficiencies)
            .Include(f => f.WeaponTypeProficiencies)
            .Include(f => f.SkillProficiencies)
            .Include(f => f.Languages)
            .Include(f => f.ToolProficiencies)
            .Include(f => f.WeaponCategoryProficiencies)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Feat with id {id} could not be found");
    }

    public async Task<ClassFeature> GetWithChoicesAsync(int id)
    {
        return await context.ClassFeatures
            .Include(f => f.AbilityIncreaseChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponTypeProficiencyChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.WeaponCategoryProficiencyChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Feat with id {id} could not be found");
    }
}