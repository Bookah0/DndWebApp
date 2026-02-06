using Api.Data;
using Api.Models.Characters;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented.Species;

public class RaceRepository(AppDbContext context) : IRaceRepository
{
    public async Task<Race> GetByIdAsync(int id) =>
        await context.Races.FirstOrDefaultAsync(r => r.Id == id)
            ?? throw new Exception($"Race with id {id} could not be found");

    public async Task<Race> GetWithTraitsAsync(int id) => 
        await context.Races
            .Include(r => r.Traits)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Race with id {id} could not be found");

    public async Task<Race> GetWithSubracesAsync(int id) =>
        await context.Races
            .Include(r => r.SubRaces)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Race with id {id} could not be found");

    public async Task<Race> GetWithAllDataAsync(int id) =>
        await context.Races
            .Include(r => r.Traits)
            .Include(r => r.SubRaces)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Race with id {id} could not be found");

    public async Task<ICollection<Race>> GetAllAsync() => await context.Races.ToListAsync();

    public async Task<Race> CreateAsync(Race entity)
    {
        await context.Races.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

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
}