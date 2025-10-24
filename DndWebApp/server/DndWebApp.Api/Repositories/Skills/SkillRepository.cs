using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Skills;

public class SkillRepository(AppDbContext context) : EfRepository<Skill>(context), ISkillRepository
{
    public async Task<Skill?> GetWithAbilityAsync(int id)
    {
        return await dbSet.Include(s => s.Ability).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Skill>> GetAllWithAbilityAsync()
    {
        return await dbSet.Include(s => s.Ability).ToListAsync();
    }
}