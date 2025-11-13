using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Classes;

public class ClassRepository : IClassRepository
{
    private readonly AppDbContext context;

    public ClassRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Class> CreateAsync(Class entity)
    {
        await context.Classes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<Class>> GetAllAsync() => await context.Classes.ToListAsync();
    public async Task<Class?> GetByIdAsync(int id) => await context.Classes.FirstOrDefaultAsync(c => c.Id == id);
    public async Task<Class?> GetByTypeAsync(ClassType type) => await context.Classes.FirstOrDefaultAsync(c => c.Type == type);

    public async Task DeleteAsync(Class entity)
    {
        context.Classes.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Class updatedEntity)
    {
        context.Classes.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<Class?> GetWithAllDataAsync(int id)
    {
        return await context.Classes
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithClassLevelsAsync(int id)
    {
        return await context.Classes
            .Include(c => c.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithClassLevelFeaturesAsync(int id)
    {
        return await context.Classes
            .Include(c => c.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithStartingEquipmentAsync(int id)
    {
        return await context.Classes
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Class>> GetAllWithAllDataAsync()
    {
        return await context.Classes
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .ToListAsync();
    }

}