using Api.Domain.Abilities.DTOs;
using Api.Domain.Abilities.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
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

    public async Task<ICollection<Ability>> GetAllAsync() => await GetAllAsync(null);

    public async Task<ICollection<Ability>> GetAllWithSkillsAsync() => 
        await context.AbilityScores
            .Include(a => a.Skills)
            .ToListAsync();

    public async Task<Ability> CreateAsync(Ability entity)
    {
        await context.AbilityScores.AddAsync(entity);
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

    public async Task<ICollection<Ability>> GetAllAsync(AbilityFilterDto? filter = null)
    {
        var query = context.AbilityScores.AsQueryable();

		if(filter is not null) 
			query = query.WhereIf(filter.Name, a => a.FullName.Contains(filter.Name!));

		var abilities = await query.ToListAsync();
        var defaultOrder = QueryExtensions.BuildSortOrder(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);
		
		return abilities
			.AsEnumerable()
			.OrderByFixed(a => a.FullName, defaultOrder)
			.ToList();
    }
}