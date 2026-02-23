using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Classes.Repositories;

public class SubclassRepository(AppDbContext context) : ISubclassRepository
{
    public async Task<Subclass> GetByIdAsync(int id) => 
        await context.Subclasses.FirstOrDefaultAsync(c => c.Id == id) 
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<Subclass> GetWithLevelsAsync(int id) => 
        await context.Subclasses
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<Subclass> GetWithLevelFeaturesAsync(int id) => 
        await context.Subclasses
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Subclass with id {id} could not be found");

    public async Task<ICollection<Subclass>> GetAllAsync() => await context.Subclasses.ToListAsync();
    
    public async Task<Subclass> CreateAsync(Subclass entity)
    {
        await context.Subclasses.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Subclass entity)
    {
        context.Subclasses.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Subclass> UpdateAsync(Subclass updatedEntity)
    {
        context.Subclasses.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<(int, ICollection<Subclass>)> GetFilteredAsync(SubclassFilterDto filter, PaginationRequestDto pagination)
    {      
        var query = context.Subclasses
            .AsQueryable()
            .WhereIf(filter.CreatedBy, i => i.CreatedBy == filter.CreatedBy)
            .WhereIf(filter.Name, s => s.Name.Contains(filter.Name!))
            .WhereIf(filter.IsSpellcaster, s => (bool)filter.IsSpellcaster! ? s.SpellcastingAbilityId != null : s.SpellcastingAbilityId == null)

            .WhereIf(filter.IsHomebrew, s => s.IsHomebrew == filter.IsHomebrew)
            .WhereIf(filter.CloningAllowed, s => s.CloningAllowed == filter.CloningAllowed)
            .SortBy(filter.SortBy, sortSelectorsMap, SortSubclassOption.Name, filter.SortDescending);
        
        var subclassCount = await query.CountAsync();
        var filteredSubclasses = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (subclassCount, filteredSubclasses);
    }

    private readonly Dictionary<string, IEnumerable<Func<Subclass, object>>> sortSelectorsMap = new()
    {
        { SortSubclassOption.Name, [(s => s.Name)] },
        { SortSubclassOption.ParentClass, [(s => s.ParentClassId)] }
    };
}