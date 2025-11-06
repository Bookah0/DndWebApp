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