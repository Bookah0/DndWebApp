using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Domain.Species.DTOs;
using Api.Domain.Species.Models;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Species.Repositories;

public class SubraceRepository(AppDbContext context) : ISubraceRepository
{
    public async Task<Subrace> GetByIdAsync(int id) => 
        await context.Subraces.FindAsync(id)
            ?? throw new Exception($"Subrace with id {id} could not be found");

    public async Task<Subrace> GetByNameAsync(string name) => 
        await context.Subraces.FirstOrDefaultAsync(r => r.Name == name) 
            ?? throw new Exception($"Subrace with name {name} could not be found");

    public async Task<Subrace> GetWithTraitsAsync(int id) =>
        await context.Subraces
            .Include(r => r.Traits)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Subrace with id {id} could not be found");

    public async Task<Subrace> GetWithAllDataAsync(int id) =>
        await context.Subraces
        .Include(r => r.Traits)
            .Include(r => r.ParentRace)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Subrace with id {id} could not be found");

    public async Task<ICollection<Subrace>> GetAllAsync() => await context.Subraces.ToListAsync();

    public async Task<Subrace> CreateAsync(Subrace entity)
    {
        await context.Subraces.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Subrace entity)
    {
        context.Subraces.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task<Subrace> UpdateAsync(Subrace updatedEntity)
    {
        context.Subraces.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

	public async Task<ICollection<Subrace>> GetAllAsync(SubraceFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Subraces.AsQueryable();

		if (filter is not null)
		{
			query = query
			.WhereIf(filter.Name, r => r.Name.Contains(filter.Name!))
			.WhereIf(filter.ParentRace, r => r.ParentRaceId == filter.ParentRace)
			.WhereIf(filter.CreatedBy, r => r.CreatedBy == filter.CreatedBy)
			.WhereIf(filter.IsHomebrew, r => r.IsHomebrew == filter.IsHomebrew)
			.WhereIf(filter.CloningAllowed, r => r.CloningAllowed == filter.CloningAllowed);
		}

		query = query.OrderByMany([(r => r.Name)], filter?.SortDescending ?? true);

		if(pagination is null)
			return await query.ToListAsync();	
			
		var subraces = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return subraces;
	}
}