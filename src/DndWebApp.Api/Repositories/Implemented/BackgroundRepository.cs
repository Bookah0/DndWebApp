using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class BackgroundRepository : IBackgroundRepository
{
    private readonly AppDbContext context;

    public BackgroundRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Background> CreateAsync(Background entity)
    {
        await context.Backgrounds.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Background entity)
    {
        context.Backgrounds.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Background updatedEntity)
    {
        context.Backgrounds.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Background>> GetMiscellaneousItemsAsync() => await context.Backgrounds.ToListAsync();
    public async Task<Background?> GetByIdAsync(int id) => await context.Backgrounds.FindAsync(id);

    public async Task<Background?> GetWithFeaturesAsync(int id)
    {
        return await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<Background?> GetWithAllDataAsync(int id)
    {
        return await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Background>> GetAllWithAllDataAsync()
    {
        return await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
            .ToListAsync();
    }
}