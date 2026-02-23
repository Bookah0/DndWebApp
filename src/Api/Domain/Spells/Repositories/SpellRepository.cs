using static Api.Domain.Shared.Utils.QueryUtil;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Spells.DTOs;
using Api.Domain.Spells.Models;
using Api.Infrastructure.Data;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Spells.Repositories;

public class SpellRepository(AppDbContext context) : ISpellRepository
{
    public async Task<Spell> GetByIdAsync(int id) => 
        await context.Spells.FindAsync(id)
            ?? throw new Exception($"Spell with id {id} could not be found");

    public async Task<Spell> GetWithClassesAsync(int id) =>
        await context.Spells
            .Include(s => s.Classes)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Spell with id {id} could not be found");

    public async Task<ICollection<Spell>> GetAllAsync() => await context.Spells.ToListAsync();
    
    public async Task<ICollection<Spell>> GetAllWithClassesAsync() =>
        await context.Spells
            .Include(s => s.Classes)
            .ToListAsync();
    
    public async Task<Spell> CreateAsync(Spell entity)
    {
        await context.Spells.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Spell entity)
    {
        context.Spells.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Spell> UpdateAsync(Spell updatedEntity)
    {
        context.Spells.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
    
    public async Task<(int, ICollection<Spell>)> GetFilteredAsync(SpellFilterDto filter, PaginationRequestDto pagination)
    {      
        var query = context.Spells
            .AsQueryable()
            .WhereIf(filter.UserId, i => i.CreatedBy == filter.UserId)
            .WhereIf(filter.Name, s => s.Name.Contains(filter.Name!))
            .WhereIf(filter.MagicSchool, s => filter.MagicSchool!.Contains(s.MagicSchool))
            // .WhereIf(filter.ClassId, s => s.Classes.Any(c => filter.ClassId!.Contains(c.Id)))
            .WhereIf(filter.Duration, s => filter.Duration!.Contains(s.Duration))
            .WhereIf(filter.CastingTime, s => filter.CastingTime!.Contains(s.CastingTime))
            .WhereIf(filter.SpellType, s => s.SpellTypes.Any(t => filter.SpellType!.Contains(t)))
            .WhereIf(filter.TargetType, s => filter.TargetType!.Contains(s.SpellTargeting.TargetType))
            .WhereIf(filter.DamageType, s => s.DamageTypes.Any(t => filter.DamageType!.Contains(t)))

            .WhereIf(filter.MinLevel, s => s.Level >= filter.MinLevel)
            .WhereIf(filter.MaxLevel, s => s.Level <= filter.MaxLevel)

            .WhereIf(filter.Range, s => filter.Range!.Contains(s.SpellTargeting.Range))
            .WhereIf(filter.MinRangeValue, s => s.SpellTargeting.RangeValue >= filter.MinRangeValue)
            .WhereIf(filter.MaxRangeValue, s => s.SpellTargeting.RangeValue <= filter.MaxRangeValue)

            .WhereIf(filter.IsHomebrew, s => s.IsHomebrew == filter.IsHomebrew)
            .WhereIf(filter.CloningAllowed, s => s.CloningAllowed == filter.CloningAllowed);

        if (filter.SortBy is not null)
            query = SortBy(query, filter.SortBy, filter.SortDescending);
        
        var spellCount = await query.CountAsync();
        var filteredSpells = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (spellCount, filteredSpells);
    }

    public IQueryable<Spell> SortBy(IQueryable<Spell> query, string sortFilter, bool descending = false)
    {
        if(!ValuesValidator.TryNormalizeValue<SortSpellOption>(sortFilter, out string? normalized))
            return context.Spells;

        return normalized switch
        {
            SortSpellOption.Name => OrderByMany(query, [(s => s.Name)], descending),
            SortSpellOption.Level => OrderByMany(query, [(s => s.Level), (s => s.Name)], descending),
            SortSpellOption.CastingTime => OrderByMany(query, [(s => s.CastingTime), (s => s.CastingTimeValue!), (s => s.Name)], descending),
            SortSpellOption.Duration => OrderByMany(query, [(s => s.Duration), (s => s.DurationValue!), (s => s.Name)], descending),
            SortSpellOption.Target => OrderByMany(query, [(s => s.SpellTargeting.TargetType), (s => s.Name)], descending),
            SortSpellOption.Range => OrderByMany(query, [(s => s.SpellTargeting.Range), (s => s.SpellTargeting.RangeValue!), (s => s.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}