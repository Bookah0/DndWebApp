using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class FeatRepository : IFeatRepository
{
    private readonly AppDbContext context;

    public FeatRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Feat?> GetWithAllDataAsync(int id)
    {
        return await context.Feats
            .AsSplitQuery()
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Feat>> GetAllWithAllDataAsync()
    {
        return await context.Feats
            .AsSplitQuery()
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .ToListAsync();
    }
}