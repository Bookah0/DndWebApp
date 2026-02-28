using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Classes.Repositories;

public class ClassRepository(AppDbContext context) : IBaseClassRepository
{
    public async Task<BaseClass> GetByIdAsync(int id) => 
        await context.Classes.FirstOrDefaultAsync(c => c.Id == id) 
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithSubclassesAsync(int id) =>
        await context.Classes
            .Include(c => c.Subclasses)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithAllDataAsync(int id) =>
        await context.Classes
            .Include(c => c.Subclasses)
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithLevelsAsync(int id) =>
        await context.Classes
            .Include(c => c.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithLevelFeaturesAsync(int id) =>
        await context.Classes
            .Include(c => c.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<BaseClass> GetWithStartingEquipmentAsync(int id) =>
        await context.Classes
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Class with id {id} could not be found");

    public async Task<ICollection<BaseClass>> GetAllAsync() => await context.Classes.ToListAsync();
    
    public async Task<ICollection<BaseClass>> GetAllWithLevelFeaturesAsync() =>
        await context.Classes
            .Include(c => c.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .ToListAsync();

    public async Task<ICollection<BaseClass>> GetAllWithAllDataAsync() =>
        await context.Classes
            .Include(c => c.Subclasses)
            .Include(c => c.ClassLevels)
            .Include(c => c.StartingEquipment)
            .Include(c => c.StartingEquipmentChoices)
            .ToListAsync();

    public async Task<bool> ExistsAsync(int id) => await context.Classes.AnyAsync(c => c.Id == id);

    public async Task<BaseClass> CreateAsync(BaseClass entity)
    {
        await context.Classes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(BaseClass entity)
    {
        context.Classes.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<BaseClass> UpdateAsync(BaseClass updatedEntity)
    {
        context.Classes.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<ICollection<BaseClass>> GetAllAsync(ClassFilterDto? filter = null, PaginationRequestDto? pagination = null)
    {      
        var query = context.Classes.AsQueryable();

		if(filter is not null)
			query = query
				.WhereIf(filter.Name, s => s.Name.Contains(filter.Name!))
				.WhereIf(filter.IsSpellcaster, s => (bool)filter.IsSpellcaster! ? s.SpellcastingAbilityId != null : s.SpellcastingAbilityId == null)

				.WhereIf(filter.CreatedBy, i => i.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, s => s.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, s => s.CloningAllowed == filter.CloningAllowed);
		
		query = query.OrderByMany([c => c.Name], filter?.SortDescending ?? true);

		if(pagination is null)
			return await query.ToListAsync();

        var filteredClasses = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return filteredClasses;
    }
}