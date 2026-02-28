using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Domain.Species.DTOs;
using Api.Domain.Species.Models;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Species.Repositories;

public class RaceRepository(AppDbContext context) : IRaceRepository
{
	public async Task<Race> GetByIdAsync(int id) =>
		await context.Races.FirstOrDefaultAsync(r => r.Id == id)
			?? throw new Exception($"Race with id {id} could not be found");

	public async Task<Race> GetWithTraitsAsync(int id) =>
		await context.Races
			.Include(r => r.Traits)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new Exception($"Race with id {id} could not be found");

	public async Task<Race> GetWithSubracesAsync(int id) =>
		await context.Races
			.Include(r => r.SubRaces)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new Exception($"Race with id {id} could not be found");

	public async Task<Race> GetWithAllDataAsync(int id) =>
		await context.Races
			.Include(r => r.Traits)
			.Include(r => r.SubRaces)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new Exception($"Race with id {id} could not be found");

	public async Task<ICollection<Race>> GetAllAsync() => await context.Races.ToListAsync();

	public async Task<Race> CreateAsync(Race entity)
	{
		await context.Races.AddAsync(entity);
		await context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(Race entity)
	{
		context.Races.Remove(entity);
		await context.SaveChangesAsync();
	}

	public async Task<Race> UpdateAsync(Race updatedEntity)
	{
		context.Races.Update(updatedEntity);
		await context.SaveChangesAsync();
		return updatedEntity;
	}

	public async Task<ICollection<Race>> GetAllAsync(RaceFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Races.AsQueryable();

		if (filter is not null)
		{
			query = query
				.WhereIf(filter.Name, r => r.Name.Contains(filter.Name!))
				.WhereIf(filter.CreatedBy, r => r.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, r => r.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, r => r.CloningAllowed == filter.CloningAllowed);
		}

		query = query.OrderByMany([(f => f.Name)], filter?.SortDescending ?? true);

		if (pagination is null)
			return await query.ToListAsync();

		var races = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return races;
	}
}