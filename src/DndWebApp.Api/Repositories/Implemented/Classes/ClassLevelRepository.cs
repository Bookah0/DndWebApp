using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Classes;

public class ClassLevelRepository : IClassLevelRepository
{
    private readonly AppDbContext context;

    public ClassLevelRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<ClassLevel> CreateAsync(ClassLevel entity)
    {
        await context.ClassLevels.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<ClassLevel>> GetMiscellaneousItemsAsync() => await context.ClassLevels.ToListAsync();
    public async Task<ClassLevel?> GetByIdAsync(int id) => await context.ClassLevels.FirstOrDefaultAsync(c => c.Id == id);

    public async Task DeleteAsync(ClassLevel entity)
    {
        context.ClassLevels.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ClassLevel updatedEntity)
    {
        context.ClassLevels.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ClassLevel?> GetWithFeaturesByClassIdAsync(int classId, int level)
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(l => l.ClassId == classId && l.Level == level);
    }

    public async Task<ClassLevel?> GetWithFeaturesAsync(int id)
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ClassLevel?> GetWitClassSpecificSlotsAtLevelAsync(int id)
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<ClassLevel?> GetWithAllDataAsync(int id)
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassLevel>> GetAllWithAllDataAsync()
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .ToListAsync();
    }
}