using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Classes;

public class ClassRepository(AppDbContext context) : IClassRepository
{
    public async Task<Class> GetByIdAsync(int id) => 
        await context.Classes.FirstOrDefaultAsync(c => c.Id == id) 
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<Class> GetWithSubclassesAsync(int id) =>
        await context.Classes
            .Include(c => c.Subclasses)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<Class> GetWithAllDataAsync(int id) =>
        await context.Classes
            .Include(c => c.Subclasses)
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<Class> GetWithLevelsAsync(int id) =>
        await context.Classes
            .Include(c => c.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<Class> GetWithClassLevelFeaturesAsync(int id) =>
        await context.Classes
            .Include(c => c.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<Class> GetWithStartingEquipmentAsync(int id) =>
        await context.Classes
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<ICollection<Class>> GetAllAsync() => await context.Classes.ToListAsync();
    
    public async Task<ICollection<Class>> GetAllWithAllDataAsync() =>
        await context.Classes
            .Include(c => c.Subclasses)
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .ToListAsync();

    public async Task<bool> ExistsAsync(int id) => await context.Classes.AnyAsync(c => c.Id == id);

    public async Task<Class> CreateAsync(Class entity)
    {
        await context.Classes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

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
}