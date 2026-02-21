using Api.Domain.Abilities.Models;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Abilities.Repositories;

public class AbilityValueRepository(AppDbContext context) : IAbilityValueRepository
{
    public async Task<AbilityValue> GetByIdAsync(int id) => 
        await context.AbilityValues.FindAsync(id)
            ?? throw new Exception($"AbilityValue with id {id} could not be found");

    public async Task<AbilityValue> GetWithAbilityAsync(int id) =>
        await context.AbilityValues
            .Include(a => a.Ability)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"AbilityValue with id {id} could not be found");

    public async Task<ICollection<AbilityValue>> GetAllAsync() => await context.AbilityValues.ToListAsync();

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
    public async Task<AbilityValue> UpdateAsync(AbilityValue updatedEntity)
    {
        context.AbilityValues.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
}