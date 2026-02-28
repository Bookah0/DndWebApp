using Api.Domain.Backgrounds.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Backgrounds.Repositories;

public class BackgroundFeatureRepository(AppDbContext context) : IFeatureRepository<BackgroundFeature, BackgroundFeatureFilterDto>
{
    public async Task<BackgroundFeature> GetByIdAsync(int id) =>
        await context.BackgroundFeatures.FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");
    
    public async Task<BackgroundFeature> GetWithAllDataAsync(int id) =>
        await context.BackgroundFeatures
            .AsSplitQuery()
            .Include(b => b.Background)
            .Include(b => b.AbilityIncreases)
            .Include(b => b.SpellsGained)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");

    public async Task<BackgroundFeature> GetWithProficienciesAsync(int id) =>
        await context.BackgroundFeatures
            .Include(f => f.AbilityIncreases)
            .Include(f => f.ArmorProficiencies)
            .Include(f => f.WeaponTypeProficiencies)
            .Include(f => f.SkillProficiencies)
            .Include(f => f.Languages)
            .Include(f => f.ToolProficiencies)
            .Include(f => f.WeaponCategoryProficiencies)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");

    public async Task<BackgroundFeature> GetWithChoicesAsync(int id) =>
        await context.BackgroundFeatures
            .Include(f => f.AbilityIncreaseChoices)
            .Include(f => f.ArmorProficiencyChoices)
            .Include(f => f.WeaponTypeProficiencyChoices)
            .Include(f => f.SkillProficiencyChoices)
            .Include(f => f.LanguageChoices)
            .Include(f => f.ToolProficiencyChoices)
            .Include(f => f.WeaponCategoryProficiencyChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException($"Background feature with id {id} could not be found");

    public async Task<ICollection<BackgroundFeature>> GetAllAsync() => await context.BackgroundFeatures.ToListAsync();
    
    public async Task<BackgroundFeature> CreateAsync(BackgroundFeature entity)
    {
        await context.BackgroundFeatures.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    
    public async Task DeleteAsync(BackgroundFeature entity)
    {
        context.BackgroundFeatures.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<BackgroundFeature> UpdateAsync(BackgroundFeature updatedEntity)
    {
        context.BackgroundFeatures.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

	public async Task<ICollection<BackgroundFeature>> GetAllAsync(BackgroundFeatureFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.BackgroundFeatures.AsQueryable();
		
		if (filter is not null)
		{
			query = query
				.WhereIf(filter.Name, t => t.Name.Contains(filter.Name!))
				.WhereIf(filter.Background, t => t.BackgroundId == filter.Background)

				.WhereIf(filter.CreatedBy, t => t.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, t => t.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, t => t.CloningAllowed == filter.CloningAllowed);
		}

		query = query.OrderByMany([(f => f.Background!.Name), (f => f.Name)], filter?.SortDescending ?? false);
		
		if(pagination is null)
			return await query.ToListAsync();
			
		var filteredFeatures = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return filteredFeatures;
	}
}