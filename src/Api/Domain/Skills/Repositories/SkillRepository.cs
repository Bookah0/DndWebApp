using System.Linq.Expressions;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Utils;
using Api.Domain.Skills.DTOs;
using Api.Domain.Skills.Models;
using Api.Infrastructure.Data;
using Api.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Skills.Repositories;

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
		await context.Skills.AddAsync(entity);
		await context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(Skill entity)
	{
		context.Skills.Remove(entity);
		await context.SaveChangesAsync();
	}
	public async Task<Skill> UpdateAsync(Skill updatedEntity)
	{
		context.Skills.Update(updatedEntity);
		await context.SaveChangesAsync();
		return updatedEntity;
	}

	public async Task<ICollection<Skill>> GetAllAsync(SkillFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Skills.AsQueryable();

		if (filter is not null)
		{
			query = query
				.WhereIf(filter.Name, s => s.Name.Contains(filter.Name!))
				.WhereIf(filter.AbilityId, s => s.AbilityId == filter.AbilityId)

				.WhereIf(filter.CreatedBy, s => s.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, s => s.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, s => s.CloningAllowed == filter.CloningAllowed);
		}

		var sortBy = ValuesValidator.NormalizeValue<SortSkillOption>(filter?.SortBy ?? SortSkillOption.Default);
		query = query.OrderByMany(sortSelectorsMap[sortBy], filter?.SortDescending ?? false);

		if(pagination is null)
			return await query.ToListAsync();

		var filteredSkills = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return filteredSkills;
	}

	private readonly Dictionary<string, IEnumerable<Expression<Func<Skill, object>>>> sortSelectorsMap = new()
	{
		{ SortSkillOption.Name, [(s => s.Name)] },
		{ SortSkillOption.Ability, [(s => s.AbilityId), (s => s.Name)] },
	};

}


