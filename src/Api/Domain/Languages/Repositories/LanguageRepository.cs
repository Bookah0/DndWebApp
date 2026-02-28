using System.Linq.Expressions;
using Api.Domain.Languages.DTOs;
using Api.Domain.Languages.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Api.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Languages.Repositories;

public class LanguageRepository(AppDbContext context) : ILanguageRepository
{
	public async Task<Language> GetByIdAsync(int id) =>
		await context.Languages.FindAsync(id)
			?? throw new Exception($"Language with id {id} could not be found");

	public async Task<Language> GetByNameAsync(string name) =>
		await context.Languages.FirstOrDefaultAsync(l => l.Name == name)
			?? throw new Exception($"Language with name {name} could not be found");

	public async Task<ICollection<Language>> GetAllAsync() => await context.Languages.ToListAsync();

	public async Task<Language> CreateAsync(Language entity)
	{
		await context.Languages.AddAsync(entity);
		await context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(Language entity)
	{
		context.Languages.Remove(entity);
		await context.SaveChangesAsync();
	}
	public async Task<Language> UpdateAsync(Language updatedEntity)
	{
		context.Languages.Update(updatedEntity);
		await context.SaveChangesAsync();
		return updatedEntity;
	}

	public async Task<ICollection<Language>> GetAllAsync(LanguageFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Languages.AsQueryable();

		if (filter is not null)
		{
			query = query
				.WhereIf(filter.Name, s => s.Name.Contains(filter.Name!))
				.WhereIf(filter.Family, s => s.Family.Contains(filter.Family!))
				.WhereIf(filter.Script, s => s.Script.Contains(filter.Script!))

				.WhereIf(filter.CreatedBy, s => s.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, s => s.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, s => s.CloningAllowed == filter.CloningAllowed);
		}

		var sortBy = ValuesValidator.NormalizeValue<SortLanguageOption>(filter?.SortBy ?? SortLanguageOption.Default);
		query = query.OrderByMany(sortSelectorsMap[sortBy], filter?.SortDescending ?? false);

		if (pagination is null)
			return await query.ToListAsync();

		var filteredLanguages = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return filteredLanguages;
	}

	private readonly Dictionary<string, IEnumerable<Expression<Func<Language, object>>>> sortSelectorsMap = new()
	{
		{ SortLanguageOption.Name, [(s => s.Name)] },
		{ SortLanguageOption.Family, [(s => s.Family), (s => s.Name)] },
		{ SortLanguageOption.Script, [(s => s.Script), (s => s.Name)] },
	};
}