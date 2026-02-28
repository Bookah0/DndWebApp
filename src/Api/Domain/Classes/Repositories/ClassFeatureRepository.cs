using Api.Domain.Classes.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Classes.Repositories;

public class ClassFeatureRepository(AppDbContext context) : IFeatureRepository<ClassFeature, ClassFeatureFilterDto>
{
	public async Task<ClassFeature> GetByIdAsync(int id) =>
		await context.ClassFeatures.FirstOrDefaultAsync(f => f.Id == id)
			?? throw new NotFoundException($"Class feature with id {id} could not be found");

	public async Task<ClassFeature> GetWithAllDataAsync(int id) =>
		await context.ClassFeatures
			.AsSplitQuery()
			.Include(f => f.Level)
			.Include(f => f.AbilityIncreases)
			.Include(f => f.SpellsGained)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Class feature with id {id} could not be found");

	public async Task<ClassFeature> GetWithProficienciesAsync(int id) =>
		await context.ClassFeatures
			.Include(f => f.AbilityIncreases)
			.Include(f => f.ArmorProficiencies)
			.Include(f => f.WeaponTypeProficiencies)
			.Include(f => f.SkillProficiencies)
			.Include(f => f.Languages)
			.Include(f => f.ToolProficiencies)
			.Include(f => f.WeaponCategoryProficiencies)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Class feature with id {id} could not be found");

	public async Task<ClassFeature> GetWithChoicesAsync(int id) =>
		await context.ClassFeatures
			.Include(f => f.AbilityIncreaseChoices)
			.Include(f => f.ArmorProficiencyChoices)
			.Include(f => f.WeaponTypeProficiencyChoices)
			.Include(f => f.SkillProficiencyChoices)
			.Include(f => f.LanguageChoices)
			.Include(f => f.ToolProficiencyChoices)
			.Include(f => f.WeaponCategoryProficiencyChoices)
			.FirstOrDefaultAsync(x => x.Id == id)
			?? throw new NotFoundException($"Class feature with id {id} could not be found");

	public async Task<ICollection<ClassFeature>> GetAllAsync() => await context.ClassFeatures.ToListAsync();

	public async Task<ClassFeature> CreateAsync(ClassFeature entity)
	{
		await context.ClassFeatures.AddAsync(entity);
		await context.SaveChangesAsync();
		return entity;
	}

	public async Task DeleteAsync(ClassFeature entity)
	{
		context.ClassFeatures.Remove(entity);
		await context.SaveChangesAsync();
	}

	public async Task<ClassFeature> UpdateAsync(ClassFeature updatedEntity)
	{
		context.ClassFeatures.Update(updatedEntity);
		await context.SaveChangesAsync();
		return updatedEntity;
	}

	public async Task<ICollection<ClassFeature>> GetAllAsync(ClassFeatureFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.ClassFeatures.AsQueryable();

		if (filter is not null)
			query = query
				.WhereIf(filter.Name, f => f.Name.Contains(filter.Name!))
				.WhereIf(filter.Class, f => f.ClassId == filter.Class)
				.WhereIf(filter.Level, f => f.LevelId == filter.Level)

				.WhereIf(filter.CreatedBy, f => f.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, f => f.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, f => f.CloningAllowed == filter.CloningAllowed);
		
		query = query.OrderByMany([(f => f.Name)], filter?.SortDescending ?? true);

		if(pagination is null)
			return await query.ToListAsync();
			
		var classFeatures = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return classFeatures;
	}
}