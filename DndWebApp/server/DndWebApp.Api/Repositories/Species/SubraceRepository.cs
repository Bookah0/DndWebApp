using System.IO.Compression;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Species;

public class SubraceRepository(AppDbContext context) : EfRepository<Subrace>(context), ISubraceRepository
{
    public async Task<SubraceDto?> GetSubraceDtoAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new SubraceDto
            {
                Id = r.Id,
                Name = r.Name,
                GeneralDescription = r.RaceDescription.GeneralDescription,
                ParentRaceId = r.ParentRaceId,
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Subrace?> GetWithAllDataAsync(int id)
    {
        return await dbSet
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<SubraceDto>> GetAllSubraceDtosAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new SubraceDto
            {
                Id = r.Id,
                Name = r.Name,
                GeneralDescription = r.RaceDescription.GeneralDescription,
                ParentRaceId = r.ParentRaceId,
            })
            .ToListAsync();
    }

    public async Task<ICollection<Subrace>> GetAllSubracesByRaceAsync(int raceId)
    {
        return await dbSet
        .Where(s => s.ParentRaceId == raceId)
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .ToListAsync();
    }

    public async Task<ICollection<Subrace>> GetAllWithAllDataAsync()
    {
        return await dbSet
        .Include(r => r.Traits)
        .Include(r => r.ParentRace)
        .ToListAsync();
    }
}