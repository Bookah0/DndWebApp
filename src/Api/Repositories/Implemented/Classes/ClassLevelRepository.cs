using Api.Data;
using Api.Models.Characters;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented.Classes;

public class ClassLevelRepository(AppDbContext context) : IClassLevelRepository
{
    public async Task<ClassLevel> GetByIdAsync(int id) =>
        await context.ClassLevels.FirstOrDefaultAsync(c => c.Id == id) 
            ?? throw new Exception($"ClassLevel with id {id} could not be found");   

    public async Task<ClassLevel> GetWithFeaturesByClassIdAsync(int classId, int level) =>
        await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(l => l.ClassId == classId && l.Level == level)
            ?? throw new Exception($"ClassLevel with ClassId {classId} and Level {level} could not be found");

    public async Task<ClassLevel> GetWithFeaturesAsync(int id) =>
        await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"ClassLevel with id {id} could not be found");
    
    public async Task<ClassLevel> GetWithAllDataAsync(int id) =>
        await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"ClassLevel with id {id} could not be found");

    public async Task<ICollection<ClassLevel>> GetAllAsync() => await context.ClassLevels.ToListAsync();    

    public async Task<ICollection<ClassLevel>> GetAllWithAllDataAsync() => 
        await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .ToListAsync();

    public async Task<ClassLevel> CreateAsync(ClassLevel entity)
    {
        await context.ClassLevels.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ClassLevel> UpdateAsync(ClassLevel updatedEntity)
    {
        context.ClassLevels.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task DeleteAsync(ClassLevel entity)
    {
        context.ClassLevels.Remove(entity);
        await context.SaveChangesAsync();
    }
}