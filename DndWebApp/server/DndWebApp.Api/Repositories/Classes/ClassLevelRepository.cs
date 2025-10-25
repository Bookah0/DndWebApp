using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Classes;

public class ClassLevelRepository(AppDbContext context) : EfRepository<ClassLevel>(context), IClassLevelRepository
{
    public async Task<ClassLevel?> GetWithNewFeaturesAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ClassLevel?> GetWithSpellSlotsPerLevelAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.SpellSlotsAtLevel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<ClassLevel?> GetWitClassSpecificSlotsAtLevelAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<ClassLevel?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.SpellSlotsAtLevel)
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassLevel>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.SpellSlotsAtLevel)
            .Include(b => b.ClassSpecificSlotsAtLevel)
            .Include(b => b.NewFeatures)
            .ToListAsync();
    }
}