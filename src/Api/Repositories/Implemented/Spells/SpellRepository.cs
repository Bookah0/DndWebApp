using Api.Data;
using Api.Middlewares.ExceptionHandling;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Spells;
using Api.Repositories.Interfaces;
using Api.Services.Util;
using Api.Validation.AllowedValues;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using static Api.Services.Util.CollectionUtil;
using static Api.Services.Util.SortUtil;
using static Api.Validation.AllowedValues.ValuesValidator;

namespace Api.Repositories.Implemented.Spells;

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
        var query = context.Spells.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(s => s.Name.Contains(filter.Name));

        if (filter.MinLevel is not null)
            query = query.Where(s => s.Level >= filter.MinLevel.Value);

        if (filter.MaxLevel is not null)
            query = query.Where(s => s.Level <= filter.MaxLevel.Value);

        if (filter.MagicSchool.HasContent())
            query = query.Where(s => filter.MagicSchool!.Contains(s.MagicSchool));

        if (filter.ClassId.HasContent())
            query = query.Where(s => s.Classes.Any(c => filter.ClassId!.Contains(c.Id)));

        if (filter.Duration.HasContent())
            query = query.Where(s => filter.Duration!.Contains(s.Duration));

        if (filter.CastingTime.HasContent())
            query = query.Where(s => filter.CastingTime!.Contains(s.CastingTime));

        if (filter.SpellType.HasContent())
            query = query.Where(s => s.SpellTypes.Any(t => filter.SpellType!.Contains(t)));

        if (filter.TargetType.HasContent())
            query = query.Where(s => filter.TargetType!.Contains(s.SpellTargeting.TargetType));

        if (filter.Range.HasContent())
            query = query.Where(s => filter.Range!.Contains(s.SpellTargeting.Range));

        if (filter.MinRangeValue is not null)
            query = query.Where(s => s.SpellTargeting.RangeValue >= filter.MinRangeValue.Value);

        if (filter.MaxRangeValue is not null)
            query = query.Where(s => s.SpellTargeting.RangeValue <= filter.MaxRangeValue.Value);

        if (filter.DamageType.HasContent())
            query = query.Where(s => s.DamageTypes.Any(t => filter.DamageType!.Contains(t)));

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
        if(!TryResolveValue<SortSpellOption>(sortFilter, out string? resolved))
            return context.Spells;

        return resolved switch
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