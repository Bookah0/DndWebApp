using static Api.Domain.Shared.Utils.QueryExtensions;
using static Api.Infrastructure.Validation.ValuesValidator;
using Api.Domain.Items.Models;
using Api.Infrastructure.Data;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Shared.Utils;
using Api.Domain.Shared.Enums.Items;
using System.Linq.Expressions;

namespace Api.Domain.Items.Repositories;

public class ToolRepository(AppDbContext context) : IToolRepository
{
	public async Task<Tool> GetByIdAsync(int id) =>
		await context.Tools.FirstOrDefaultAsync(t => t.Id == id)
			?? throw new Exception($"Tool with id {id} could not be found");

	public async Task<Tool> GetWithAllDataAsync(int id) =>
		await context.Tools
			.Include(t => t.ToolProperties)
			.Include(t => t.Activities)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new Exception($"Tool with id {id} could not be found");

	public async Task<ICollection<Tool>> GetAllAsync() => await context.Tools.ToListAsync();

	public async Task<ICollection<Tool>> GetAllWithAllDataAsync() =>
		await context.Tools
			.Include(t => t.ToolProperties)
			.Include(t => t.Activities)
			.ToListAsync();

	public async Task<Tool> CreateAsync(Tool entity)
	{
		await context.Tools.AddAsync(entity);
		await context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(Tool entity)
	{
		context.Tools.Remove(entity);
		await context.SaveChangesAsync();
	}

	public async Task<Tool> UpdateAsync(Tool updatedEntity)
	{
		context.Tools.Update(updatedEntity);
		await context.SaveChangesAsync();
		return updatedEntity;
	}

	public async Task<ICollection<Tool>> GetAllAsync(ToolFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Tools.AsQueryable();

		if (filter is not null)
		{
			filter.Rarity = NormalizeValue<ItemRarity>(filter.Rarity);
			filter.ToolCategory = NormalizeValue<ToolCategory>(filter.ToolCategory);

			query = query
				.WhereIf(filter.Name, t => t.Name.Contains(filter.Name!))

				.WhereIf(filter.Rarity, t => filter.Rarity.Contains(t.Rarity))
				.WhereIf(filter.ToolCategory, t => filter.ToolCategory.Contains(t.ToolCategory!))

				.WhereIf(filter.MinWeight, t => t.Weight >= filter.MinWeight)
				.WhereIf(filter.MaxWeight, t => t.Weight <= filter.MaxWeight)
				.WhereIf(filter.MinValue, t => t.Value >= filter.MinValue)
				.WhereIf(filter.MaxValue, t => t.Value <= filter.MaxValue)

				.WhereIf(filter.CreatedBy, i => i.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, t => t.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, t => t.CloningAllowed == filter.CloningAllowed);
		}

		var sortBy = NormalizeValue<SortToolOption>(filter?.SortBy ?? SortToolOption.Default);
		query = query.OrderByMany(sortSelectorsMap[sortBy], filter?.SortDescending ?? true);

		if (pagination is null)
			return await query.ToListAsync();

		var filteredItems = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return filteredItems;
	}

	private readonly Dictionary<string, IEnumerable<Expression<Func<Tool, object>>>> sortSelectorsMap = new()
	{
		{ SortToolOption.Name, [(s => s.Name)] },
		{ SortToolOption.Category, [(s => s.ToolCategory), (s => s.Name)] },
		{ SortToolOption.Value, [(s => s.Value!), (s => s.Name)] },
		{ SortToolOption.Rarity, [(s => s.Rarity == null), (s => s.Rarity!), (s => s.Name)] },
	};
}