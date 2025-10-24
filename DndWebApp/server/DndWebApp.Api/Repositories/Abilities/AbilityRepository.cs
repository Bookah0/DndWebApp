using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Abilities;

public class AbilityRepository(AppDbContext context) : EfRepository<Ability>(context), IAbilityRepository
{
    public async Task<AbilityDto?> GetPrimitiveDataAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(a => new AbilityDto
            {
                Id = a.Id,
                FullName = a.FullName,
                ShortName = a.ShortName,
                Description = a.Description
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Ability?> GetWithSkillsAsync(int id)
    {
        return await dbSet
            .Include(a => a.Skills)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<AbilityDto>> GetAllPrimitiveDataAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(a => new AbilityDto
            {
                Id = a.Id,
                FullName = a.FullName,
                ShortName = a.ShortName,
                Description = a.Description
            })
            .ToListAsync();
    }

    public async Task<ICollection<Ability>> GetAllWithSkillsAsync()
    {
        return await dbSet
            .Include(a => a.Skills)
            .ToListAsync();
    }
}