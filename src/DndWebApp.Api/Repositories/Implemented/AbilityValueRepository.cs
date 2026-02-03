using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class AbilityValueRepository : IAbilityValueRepository
{
    private readonly AppDbContext context;

    public AbilityValueRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<AbilityValue> CreateAsync(AbilityValue entity)
    {
        await context.AbilityValues.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(AbilityValue entity)
    {
        context.AbilityValues.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(AbilityValue updatedEntity)
    {
        context.AbilityValues.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<AbilityValue>> GetAllAsync() => await context.AbilityValues.ToListAsync();
    public async Task<AbilityValue?> GetByIdAsync(int id) => await context.AbilityValues.FindAsync(id);

    public async Task<AbilityValue?> GetWithAbilityAsync(int id)
    {
        return await context.AbilityValues
            .Include(a => a.Ability)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

}