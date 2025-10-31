using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Classes;

public class ClassLevelRepository : IClassLevelRepository
{
    private AppDbContext context;
    private IRepository<ClassLevel> baseRepo;

    public ClassLevelRepository(AppDbContext context, IRepository<ClassLevel> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<ClassLevel> CreateAsync(ClassLevel entity) => await baseRepo.CreateAsync(entity);
    public async Task<ClassLevel?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<ClassLevel>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(ClassLevel updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(ClassLevel entity) => await baseRepo.DeleteAsync(entity);

    public async Task<ClassLevel?> GetWithNewFeaturesAsync(int id)
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ClassLevel?> GetWithSpellSlotsPerLevelAsync(int id)
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.SpellSlotsAtLevel)
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
            .Include(b => b.SpellSlotsAtLevel)
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassLevel>> GetAllWithAllDataAsync()
    {
        return await context.ClassLevels
            .AsSplitQuery()
            .Include(b => b.SpellSlotsAtLevel)
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .ToListAsync();
    }
}