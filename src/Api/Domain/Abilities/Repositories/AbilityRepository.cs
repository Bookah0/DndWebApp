using Api.Domain.Abilities.Models;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Abilities.Repositories;

public class AbilityRepository(AppDbContext context) : IAbilityRepository
{
    public async Task<Ability> GetByIdAsync(int id) => 
        await context.AbilityScores.FindAsync(id) 
            ?? throw new Exception($"Ability with id {id} could not be found");

    public async Task<Ability> GetByShortNameAsync(string name) => 
        await context.AbilityScores.FirstOrDefaultAsync(a => a.ShortName.ToLower().Equals(name.ToLower()))
            ?? throw new Exception($"Ability with short name {name} could not be found");

    public async Task<Ability> GetWithSkillsAsync(int id) =>
        await context.AbilityScores
            .Include(a => a.Skills)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Ability with id {id} could not be found");

    public async Task<ICollection<Ability>> GetAllAsync() => await context.AbilityScores.ToListAsync();

    public async Task<ICollection<Ability>> GetAllWithSkillsAsync() => 
        await context.AbilityScores
            .Include(a => a.Skills)
            .ToListAsync();

    public async Task<Ability> CreateAsync(Ability entity)
    {
        await context.AbilityScores.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Ability entity)
    {
        context.AbilityScores.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task<Ability> UpdateAsync(Ability updatedEntity)
    {
        context.AbilityScores.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
}