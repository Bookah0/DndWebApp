using System.IO.Compression;
using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class SubraceRepository(AppDbContext context) : EfRepository<Subrace>(context)
{
    public override async Task<Subrace?> GetByIdAsync(int id)
    {
        return await dbSet
            .Include(r => r.ParentRace)
            .Include(r => r.Traits)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<ICollection<Subrace>> GetAllAsync()
    {
        return await dbSet
            .Include(r => r.ParentRace)
            .Include(r => r.Traits)
            .ToListAsync();
    }
}