using Api.Domain.Classes.Models;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Classes.Repositories;

public class SubclassRepository(AppDbContext context) : ISubclassRepository
{
    public async Task<Subclass> GetByIdAsync(int id) => 
        await context.Subclasses.FirstOrDefaultAsync(c => c.Id == id) 
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<Subclass> GetWithLevelsAsync(int id) => 
        await context.Subclasses
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<Subclass> GetWithLevelFeaturesAsync(int id) => 
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

    public async Task<Subclass> UpdateAsync(Subclass updatedEntity)
    {
        context.Subclasses.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
}