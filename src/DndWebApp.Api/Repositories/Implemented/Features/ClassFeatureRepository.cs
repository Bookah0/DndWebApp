using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class ClassFeatureRepository : IClassFeatureRepository
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

    public async Task<ICollection<ClassFeature>> GetMiscellaneousItemsAsync() => await context.ClassFeatures.ToListAsync();
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
            .Include(f => f.ClassLevel)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassFeature>> GetAllWithAllDataAsync()
    {
        return await context.ClassFeatures
            .AsSplitQuery()
            .Include(f => f.ClassLevel)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .ToListAsync();
    }
}