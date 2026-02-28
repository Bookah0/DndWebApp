using Api.Domain.Backgrounds.DTOs;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Backgrounds.Repositories;

public class BackgroundRepository(AppDbContext context) : IBackgroundRepository
{
    public async Task<Background> GetByIdAsync(int id) => 
        await context.Backgrounds.FindAsync(id)
        ?? throw new Exception($"Background with id {id} could not be found");

    public async Task<Background> GetWithFeaturesAsync(int id) =>
        await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Background with id {id} could not be found");
    
    public async Task<Background> GetWithAllDataAsync(int id) =>
        await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Background with id {id} could not be found");

    public async Task<ICollection<Background>> GetAllAsync() => await context.Backgrounds.ToListAsync();

    public async Task<ICollection<Background>> GetAllWithAllDataAsync() =>
        await context.Backgrounds
            .AsSplitQuery()
            .Include(b => b.Features)
            .Include(b => b.StartingItems)
            .Include(b => b.StartingItemsOptions)
            .ToListAsync();
    
    public async Task<Background> CreateAsync(Background entity)
    {
        await context.Backgrounds.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Background entity)
    {
        context.Backgrounds.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task<Background> UpdateAsync(Background updatedEntity)
    {
        context.Backgrounds.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

	public async Task<ICollection<Background>> GetAllAsync(BackgroundFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Backgrounds.AsQueryable();

		if(filter is not null)
			query = query
				.WhereIf(filter.Name, b => b.Name.Contains(filter.Name!))
				.WhereIf(filter.CreatedBy, b => b.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, b => b.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, b => b.CloningAllowed == filter.CloningAllowed);
		
		query = query.OrderByMany([b => b.Name], filter?.SortDescending ?? true);
		
		if(pagination is null)
			return await query.ToListAsync();

        var filteredBackgrounds = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return filteredBackgrounds;
	}
}