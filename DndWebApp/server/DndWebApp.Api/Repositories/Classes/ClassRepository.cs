using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Classes;

public class ClassRepository : IClassRepository
{
    private AppDbContext context;
    private IRepository<Class> baseRepo;

    public ClassRepository(AppDbContext context, IRepository<Class> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Class> CreateAsync(Class entity) => await baseRepo.CreateAsync(entity);
    public async Task<Class?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Class>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Class updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Class entity) => await baseRepo.DeleteAsync(entity);

    public async Task<ClassDto?> GetDtoAsync(int id)
    {
        return await context.Classes
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

    public async Task<ICollection<ClassDto>> GetAllDtosAsync()
    {
        return await context.Classes
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
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithClassLevelsAsync(int id)
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithStartingEquipmentAsync(int id)
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.Options)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Class>> GetAllWithAllDataAsync()
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.Options)
            .ToListAsync();
    }

    public Task<ICollection<Class>> FilterAllAsync(SpellFilter spellFilter)
    {
        throw new NotImplementedException();
    }
}