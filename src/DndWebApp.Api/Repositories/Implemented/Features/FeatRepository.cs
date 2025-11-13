using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class FeatRepository : IFeatRepository
{
    private readonly AppDbContext context;

    public FeatRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Feat> CreateAsync(Feat entity)
    {
        await context.Feats.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<Feat>> GetAllAsync() => await context.Feats.ToListAsync();
    public async Task<Feat?> GetByIdAsync(int id) => await context.Feats.FirstOrDefaultAsync(f => f.Id == id);
    public async Task<Feat?> GetByNameAsync(string name) => await context.Feats.FirstOrDefaultAsync(f => f.Name == name);


    public async Task DeleteAsync(Feat entity)
    {
        context.Feats.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Feat updatedEntity)
    {
        context.Feats.Update(updatedEntity);
        await context.SaveChangesAsync();
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