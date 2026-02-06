using Api.Data;
using Api.Models.Characters;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented;

public class SkillRepository(AppDbContext context) : ISkillRepository
{
    public async Task<Skill> GetByIdAsync(int id) => 
        await context.Skills.FindAsync(id)
            ?? throw new Exception($"Skill with id {id} could not be found");

    public async Task<Skill> GetByNameAsync(string name) => 
        await context.Skills.FirstOrDefaultAsync(s => s.Name == name)
            ?? throw new Exception($"Skill with name {name} could not be found");

    public async Task<Skill> GetWithAbilityAsync(int id) =>
        await context.Skills
            .Include(s => s.Ability)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Skill with id {id} could not be found");

    public async Task<ICollection<Skill>> GetAllAsync() => await context.Skills.ToListAsync();

    public async Task<ICollection<Skill>> GetAllWithAbilityAsync() =>
        await context.Skills
            .Include(s => s.Ability)
            .ToListAsync();

    public async Task<Skill> CreateAsync(Skill entity)
    {
        await context.Skills.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Skill entity)
    {
        context.Skills.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Skill updatedEntity)
    {
        context.Skills.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
}