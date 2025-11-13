using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Species;

public class RaceRepository : IRaceRepository
{
    private readonly AppDbContext context;

    public RaceRepository(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<Race> CreateAsync(Race entity)
    {
        await context.Races.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<Race>> GetAllAsync() => await context.Races.ToListAsync();
    public async Task<Race?> GetByIdAsync(int id) => await context.Races.FirstOrDefaultAsync(r => r.Id == id);
    public async Task<Race?> GetByTypeAsync(RaceType type) => await context.Races.FirstOrDefaultAsync(r => r.Type == type);

    public async Task DeleteAsync(Race entity)
    {
        context.Races.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Race updatedEntity)
    {
        context.Races.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
    
    public async Task<Race?> GetWithTraitsAsync(int id)
    {
        return await context.Races
        .Include(r => r.Traits)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Race?> GetWithAllDataAsync(int id)
    {
        return await context.Races
        .Include(r => r.Traits)
        .Include(r => r.SubRaces)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Race>> GetAllWithAllDataAsync()
    {
        return await context.Races
        .Include(r => r.Traits)
        .Include(r => r.SubRaces)
        .ToListAsync();
    }
}