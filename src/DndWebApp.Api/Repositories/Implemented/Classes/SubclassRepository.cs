using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Classes;

public class SubclassRepository(AppDbContext context) : ISubclassRepository
{
    public async Task<Subclass> GetByIdAsync(int id) => 
        await context.Subclasses.FirstOrDefaultAsync(c => c.Id == id) 
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<Subclass> GetWithClassLevelsAsync(int id) => 
        await context.Subclasses
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<Subclass> GetWithClassLevelFeaturesAsync(int id) => 
        await context.Subclasses
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<ICollection<Subclass>> GetAllAsync() => await context.Subclasses.ToListAsync();
    
    public async Task<Subclass> CreateAsync(Subclass entity)
    {
        await context.Subclasses.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Subclass entity)
    {
        context.Subclasses.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subclass updatedEntity)
    {
        context.Subclasses.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
}