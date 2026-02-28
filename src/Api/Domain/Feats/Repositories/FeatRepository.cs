using Api.Domain.Feats.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Feats.Repositories;

public class FeatRepository(AppDbContext context) : IFeatureRepository<Feat, FeatFilterDto>
{
	public async Task<Feat> GetByIdAsync(int id) =>
		await context.Feats.FirstOrDefaultAsync(f => f.Id == id)
			?? throw new NotFoundException($"Feat with id {id} could not be found");

	public async Task<Feat> GetWithAllDataAsync(int id) =>
		await context.Feats
			.AsSplitQuery()
			.Include(f => f.AbilityIncreases)
			.Include(f => f.SpellsGained)
			.Include(f => f.AbilityIncreases)
			.Include(f => f.ArmorProficiencies)
			.Include(f => f.WeaponTypeProficiencies)
			.Include(f => f.SkillProficiencies)
			.Include(f => f.Languages)
			.Include(f => f.ToolProficiencies)
			.Include(f => f.WeaponCategoryProficiencies)
			.Include(f => f.AbilityIncreaseChoices)
			.Include(f => f.ArmorProficiencyChoices)
			.Include(f => f.WeaponTypeProficiencyChoices)
			.Include(f => f.SkillProficiencyChoices)
			.Include(f => f.LanguageChoices)
			.Include(f => f.ToolProficiencyChoices)
			.Include(f => f.WeaponCategoryProficiencyChoices)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Feat with id {id} could not be found");

	public async Task<Feat> GetWithProficienciesAsync(int id) =>
		await context.Feats
			.Include(f => f.AbilityIncreases)
			.Include(f => f.ArmorProficiencies)
			.Include(f => f.WeaponTypeProficiencies)
			.Include(f => f.SkillProficiencies)
			.Include(f => f.Languages)
			.Include(f => f.ToolProficiencies)
			.Include(f => f.WeaponCategoryProficiencies)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Feat with id {id} could not be found");

	public async Task<Feat> GetWithChoicesAsync(int id) =>
		await context.Feats
			.Include(f => f.AbilityIncreaseChoices)
			.Include(f => f.ArmorProficiencyChoices)
			.Include(f => f.WeaponTypeProficiencyChoices)
			.Include(f => f.SkillProficiencyChoices)
			.Include(f => f.LanguageChoices)
			.Include(f => f.ToolProficiencyChoices)
			.Include(f => f.WeaponCategoryProficiencyChoices)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Feat with id {id} could not be found");

	public async Task<ICollection<Feat>> GetAllAsync() => await context.Feats.ToListAsync();

	public async Task<Feat> CreateAsync(Feat entity)
	{
		await context.Feats.AddAsync(entity);
		await context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(Feat entity)
	{
		context.Feats.Remove(entity);
		await context.SaveChangesAsync();
	}

	public async Task<Feat> UpdateAsync(Feat updatedEntity)
	{
		context.Feats.Update(updatedEntity);
		await context.SaveChangesAsync();
		return updatedEntity;
	}

	public async Task<ICollection<Feat>> GetAllAsync(FeatFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Feats.AsQueryable();

		if (filter is not null)
		{
			query = query
			.WhereIf(filter.Name, f => f.Name.Contains(filter.Name!))
			.WhereIf(filter.CreatedBy, f => f.CreatedBy == filter.CreatedBy)
			.WhereIf(filter.IsHomebrew, f => f.IsHomebrew == filter.IsHomebrew)
			.WhereIf(filter.CloningAllowed, f => f.CloningAllowed == filter.CloningAllowed);

		}
		query = query.OrderByMany([f => f.Name], filter?.SortDescending ?? true);

		if (pagination is null)
			return await query.ToListAsync();

		var feats = await query
			.OrderBy(f => f.Name)
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return feats;
	}
}