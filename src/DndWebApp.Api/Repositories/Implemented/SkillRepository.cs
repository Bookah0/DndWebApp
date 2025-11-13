using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class SkillRepository : ISkillRepository
{
    private readonly AppDbContext context;

    public SkillRepository(AppDbContext context)
    {
        this.context = context;
    }

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

    public async Task<ICollection<Skill>> GetAllAsync() => await context.Skills.ToListAsync();
    public async Task<Skill?> GetByIdAsync(int id) => await context.Skills.FindAsync(id);
    public async Task<Skill?> GetByTypeAsync(SkillType type) => await context.Skills.FirstOrDefaultAsync(s => s.Type == type);

    public async Task<Skill?> GetWithAbilityAsync(int id)
    {
        return await context.Skills
            .Include(s => s.Ability)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Skill>> GetAllWithAbilityAsync()
    {
        return await context.Skills
            .Include(s => s.Ability)
            .ToListAsync();
    }
}