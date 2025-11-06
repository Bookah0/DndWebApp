using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class BackgroundFeatureRepository : IBackgroundFeatureRepository
{
    private readonly AppDbContext context;

    public BackgroundFeatureRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<BackgroundFeature?> GetWithAllDataAsync(int id)
    {
        return await context.BackgroundFeatures
            .AsSplitQuery()
            .Include(b => b.Background)
            .Include(b => b.AbilityIncreases)
            .Include(b => b.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<BackgroundFeature>> GetAllWithAllDataAsync()
    {
        return await context.BackgroundFeatures
            .AsSplitQuery()
            .Include(b => b.Background)
            .Include(b => b.AbilityIncreases)
            .Include(b => b.SpellsGained)
            .ToListAsync();
    }
}