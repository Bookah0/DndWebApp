using Api.Domain.Items.Models;
using Api.Infrastructure.Data;
using Api.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Infrastructure.Middleware.ExceptionHandling;
using static Api.Domain.Shared.Utils.QueryUtil;

namespace Api.Domain.Items.Repositories;

public class ArmorRepository(AppDbContext context) : IArmorRepository
{
    public async Task<Armor> GetByIdAsync(int id) => 
        await context.Armor.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new Exception($"Armor with id {id} could not be found");

    public async Task<ICollection<Armor>> GetAllAsync() => await context.Armor.ToListAsync();

    public async Task<Armor> CreateAsync(Armor entity)
    {
        await context.Armor.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

        public async Task DeleteAsync(Armor entity)
    {
        context.Armor.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Armor> UpdateAsync(Armor updatedEntity)
    {
        context.Armor.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<(int, ICollection<Armor>)> GetFilteredAsync(ArmorFilterDto filter, PaginationRequestDto pagination)
    {      
        var query = context.Armor
            .AsQueryable()
            .WhereIf(filter.Name, w => w.Name.Contains(filter.Name!))
            .WhereIf(filter.Category, w => w.Categories.Any(c => filter.Category!.Contains(c)))
            .WhereIf(filter.Rarity, w => w.Rarity.Contains(filter.Rarity!)) 
            .WhereIf(filter.RequiresAttunement, w => w.RequiresAttunement == filter.RequiresAttunement)
            
            .WhereIf(filter.MinWeight, w => w.Weight >= filter.MinWeight)
            .WhereIf(filter.MaxWeight, w => w.Weight <= filter.MaxWeight)
            .WhereIf(filter.MinValue, w => w.Value >= filter.MinValue)
            .WhereIf(filter.MaxValue, w => w.Value <= filter.MaxValue)
           
            .WhereIf(filter.ArmorCategory, w => filter.ArmorCategory!.Contains(w.ArmorCategory))
            .WhereIf(filter.MaxAC, w => w.BaseArmorClass <= filter.MaxAC)
            .WhereIf(filter.MinAC, w => w.BaseArmorClass >= filter.MinAC)
            .WhereIf(filter.StealthDisadvantage, a => a.StealthDisadvantage == filter.StealthDisadvantage)
            .WhereIf(filter.StrengthScoreRequired, a => a.StrengthScoreRequired.HasValue)
            
            .WhereIf(filter.IsHomebrew, w => w.IsHomebrew == filter.IsHomebrew)
            .WhereIf(filter.CloningAllowed, w => w.CloningAllowed == filter.CloningAllowed);

        if (filter.SortBy is not null)
            query = SortBy(query, filter.SortBy, filter.SortDescending);
        
        var armorCount = await query.CountAsync();
        var filteredArmor = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (armorCount, filteredArmor);
    }

    public IQueryable<Armor> SortBy(IQueryable<Armor> query, string sortFilter, bool descending = false)
    {
        if(!ValuesValidator.TryNormalizeValue<SortArmorOption>(sortFilter, out string? normalized))
            return context.Armor;

        return normalized switch
        {
            SortArmorOption.Name => OrderByMany(query, [(i => i.Name)], descending),
            SortArmorOption.Category => OrderByMany(query, [(i => i.ArmorCategory), (i => i.Name)], descending),
            SortArmorOption.AC => OrderByMany(query, [(i => i.BaseArmorClass), (i => i.Name)], descending),
            SortArmorOption.Value => OrderByMany(query, [(i => i.Value!), (i => i.Name)], descending),
            SortArmorOption.Weight => OrderByMany(query, [(i => i.Weight!), (i => i.Name)], descending),
            SortArmorOption.Rarity => OrderByMany(query, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}