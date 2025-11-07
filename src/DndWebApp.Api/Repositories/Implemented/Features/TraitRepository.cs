using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Features;

public class TraitRepository : ITraitRepository
{
    private readonly AppDbContext context;

    public TraitRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Trait> CreateAsync(Trait entity)
    {
        await context.Traits.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<Trait>> GetAllAsync() => await context.Traits.ToListAsync();
    public async Task<Trait?> GetByIdAsync(int id) => await context.Traits.FirstOrDefaultAsync(t => t.Id == id);

    public async Task DeleteAsync(Trait entity)
    {
        context.Traits.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Trait updatedEntity)
    {
        context.Traits.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<Trait?> GetWithAllDataAsync(int id)
    {
        return await context.Traits
            .AsSplitQuery()
            .Include(t => t.FromRace)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Trait>> GetAllWithAllDataAsync()
    {
        return await context.Traits
            .AsSplitQuery()
            .Include(t => t.FromRace)
            .Include(f => f.AbilityIncreases)
            .Include(f => f.SpellsGained)
            .ToListAsync();
    }
}