using System.IO.Compression;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Species;

public class SubraceRepository : ISubraceRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Subrace> baseRepo;

    public SubraceRepository(AppDbContext context, IRepository<Subrace> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Subrace> CreateAsync(Subrace entity) => await baseRepo.CreateAsync(entity);
    public async Task<Subrace?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Subrace>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Subrace updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Subrace entity) => await baseRepo.DeleteAsync(entity);    

    public async Task<Subrace?> GetWithAllDataAsync(int id)
    {
        return await context.SubRaces
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Subrace>> GetAllSubracesByRaceAsync(int raceId)
    {
        return await context.SubRaces
        .Where(s => s.ParentRaceId == raceId)
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .ToListAsync();
    }

    public async Task<ICollection<Subrace>> GetAllWithAllDataAsync()
    {
        return await context.SubRaces
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .ToListAsync();
    }
}