using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories;

public class AbilityRepository(AppDbContext context) : EfRepository<Ability>(context)
{
    public override async Task<Ability?> GetByIdAsync(int id)
    {
        return await dbSet.Include(a => a.Skills).FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<ICollection<Ability>> GetAllAsync()
    {
        return await dbSet.Include(a => a.Skills).ToListAsync();
    }
}