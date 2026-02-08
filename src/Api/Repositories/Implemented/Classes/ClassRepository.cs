using Api.Data;
using Api.Models.Characters;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented.Classes;

public class ClassRepository(AppDbContext context) : IBaseClassRepository
{
    public async Task<BaseClass> GetByIdAsync(int id) => 
        await context.Classes.FirstOrDefaultAsync(c => c.Id == id) 
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithSubclassesAsync(int id) =>
        await context.Classes
            .Include(c => c.Subclasses)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithAllDataAsync(int id) =>
        await context.Classes
            .Include(c => c.Subclasses)
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithLevelsAsync(int id) =>
        await context.Classes
            .Include(c => c.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithClassLevelFeaturesAsync(int id) =>
        await context.Classes
            .Include(c => c.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithStartingEquipmentAsync(int id) =>
        await context.Classes
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<ICollection<BaseClass>> GetAllAsync() => await context.Classes.ToListAsync();
    
    public async Task<ICollection<BaseClass>> GetAllWithAllDataAsync() =>
        await context.Classes
            .Include(c => c.Subclasses)
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .ToListAsync();

    public async Task<bool> ExistsAsync(int id) => await context.Classes.AnyAsync(c => c.Id == id);

    public async Task<BaseClass> CreateAsync(BaseClass entity)
    {
        await context.Classes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(BaseClass entity)
    {
        context.Classes.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<BaseClass> UpdateAsync(BaseClass updatedEntity)
    {
        context.Classes.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
}