using Api.Data;
using Api.Models.Characters;
using Api.Models.DTOs;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented;

public class BackgroundRepository(AppDbContext context) : IBackgroundRepository
{
    public async Task<Background> GetByIdAsync(int id) => 
        await context.Backgrounds.FindAsync(id)
        ?? throw new Exception($"Background with id {id} could not be found");

    public async Task<Background> GetWithFeaturesAsync(int id) =>
        await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Background with id {id} could not be found");
    
    public async Task<Background> GetWithAllDataAsync(int id) =>
        await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Background with id {id} could not be found");

    public async Task<ICollection<Background>> GetAllAsync() => await context.Backgrounds.ToListAsync();
    
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
}