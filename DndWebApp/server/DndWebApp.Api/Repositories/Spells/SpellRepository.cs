using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Spells;

public class SpellRepository(AppDbContext context) : EfRepository<Spell>(context), ISpellRepository
{
    public async Task<Spell?> GetWithClassesAsync(int id)
    {
        return await dbSet
            .Include(s => s.Classes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Spell>> GetAllWithClassesAsync()
    {
        return await dbSet
            .Include(s => s.Classes)
            .ToListAsync();
    }
}