using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class AbilityRepository : IAbilityRepository
{
    private readonly AppDbContext context;

    public AbilityRepository(AppDbContext context)
    {
        this.context = context;
    }

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
    public async Task UpdateAsync(Ability updatedEntity)
    {
        context.AbilityScores.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Ability>> GetAllAsync() => await context.AbilityScores.ToListAsync();
    public async Task<Ability?> GetByIdAsync(int id) => await context.AbilityScores.FindAsync(id);
    public async Task<Ability?> GetByTypeAsync(AbilityShortType shortType) => await context.AbilityScores.FirstOrDefaultAsync(a => a.ShortType == shortType);
    public async Task<Ability?> GetByTypeAsync(AbilityType type) => await context.AbilityScores.FirstOrDefaultAsync(a => a.Type == type);

    public async Task<Ability?> GetWithSkillsAsync(int id)
    {
        return await context.AbilityScores
            .Include(a => a.Skills)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Ability>> GetAllWithSkillsAsync()
    {
        return await context.AbilityScores
            .Include(a => a.Skills)
            .ToListAsync();
    }

}