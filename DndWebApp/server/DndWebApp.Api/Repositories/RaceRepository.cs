using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class RaceRepository(AppDbContext context) : EfRepository<Race>(context)
{
    public override async Task<Race?> GetByIdAsync(int id)
    {
        return await dbSet
        .Include(r => r.SubRaces)
        .Include(r => r.Traits)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<ICollection<Race>> GetAllAsync()
    {
        return await dbSet
        .Include(r => r.SubRaces)
        .Include(r => r.Traits)
        .ToListAsync();
    }
}