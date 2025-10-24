using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Species;

public class RaceRepository(AppDbContext context) : EfRepository<Race>(context), IRaceRepository
{
    public async Task<RaceDto?> GetRaceDtoAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new RaceDto
            {
                Id = r.Id,
                Name = r.Name,
                GeneralDescription = r.RaceDescription.GeneralDescription,
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Race?> GetWithAllDataAsync(int id)
    {
        return await dbSet
        .Include(r => r.Traits)
        .Include(r => r.SubRaces)
        .FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<ICollection<RaceDto>> GetAllRaceDtosAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new RaceDto
            {
                Id = r.Id,
                Name = r.Name,
                GeneralDescription = r.RaceDescription.GeneralDescription,
            })
            .ToListAsync();
    }

    public async Task<ICollection<Race>> GetAllWithAllDataAsync()
    {
        return await dbSet
        .Include(r => r.Traits)
        .Include(r => r.SubRaces)
        .ToListAsync();
    }
}