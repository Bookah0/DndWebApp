using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Classes;

public class ClassRepository(AppDbContext context) : EfRepository<Class>(context), IClassRepository
{
    public async Task<ClassDto?> GetClassDtoAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsHomebrew = c.IsHomebrew,
                HitDie = c.HitDie
            })
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<ClassDto>> GetAllClassDtoDataAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsHomebrew = c.IsHomebrew,
                HitDie = c.HitDie
            })
            .ToListAsync();
    }

    public async Task<Class?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithClassLevelsAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithStartingEquipmentAsync(int id)
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Class>> GetAllWithAllDataAsync()
    {
        return await dbSet
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.Options)
            .ToListAsync();
    }
}