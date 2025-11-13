using System.IO.Compression;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Species;

public class SubraceRepository : ISubraceRepository
{
    private readonly AppDbContext context;

    public SubraceRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Subrace> CreateAsync(Subrace entity)
    {
        await context.Subraces.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Subrace entity)
    {
        context.Subraces.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Subrace updatedEntity)
    {
        context.Subraces.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Subrace>> GetAllAsync() => await context.Subraces.ToListAsync();
    public async Task<Subrace?> GetByIdAsync(int id) => await context.Subraces.FindAsync(id);
    public async Task<Subrace?> GetByNameAsync(string name) => await context.Subraces.FirstOrDefaultAsync(r => r.Name == name);

    public async Task<Subrace?> GetWithTraitsAsync(int id)
    {
        return await context.Subraces
        .Include(r => r.Traits)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Subrace?> GetWithAllDataAsync(int id)
    {
        return await context.Subraces
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Subrace>> GetAllSubracesByRaceAsync(int raceId)
    {
        return await context.Subraces
        .Where(s => s.ParentRaceId == raceId)
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .ToListAsync();
    }

    public async Task<ICollection<Subrace>> GetAllWithAllDataAsync()
    {
        return await context.Subraces
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .ToListAsync();
    }
}