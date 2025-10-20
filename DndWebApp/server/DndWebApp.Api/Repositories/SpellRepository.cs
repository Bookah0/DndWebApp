using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class SpellRepository(AppDbContext context) : EfRepository<Spell>(context)
{
    public override async Task<Spell?> GetByIdAsync(int id)
    {
        return await dbSet.Include(s => s.Classes).FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<ICollection<Spell>> GetAllAsync()
    {
        return await dbSet.Include(s => s.Classes).ToListAsync();
    }
}