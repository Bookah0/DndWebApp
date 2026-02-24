using Api.Domain.Items.Models;
using Api.Infrastructure.Data;
using Api.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Infrastructure.Middleware.ExceptionHandling;
using static Api.Domain.Shared.Utils.QueryUtil;
using Api.Domain.Shared.Utils;

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
        var normalizedSortBy = ValuesValidator.NormalizeValue<SortArmorOption>(filter.SortBy ?? SortArmorOption.Default);

        var query = context.Armor
            .AsQueryable()
            .WhereIf(filter.CreatedBy, i => i.CreatedBy == filter.CreatedBy)
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
            .WhereIf(filter.CloningAllowed, w => w.CloningAllowed == filter.CloningAllowed)
            .SortBy(normalizedSortBy, sortSelectorsMap, SortArmorOption.Default, filter.SortDescending);    
        
        var armorCount = await query.CountAsync();
        var filteredArmor = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (armorCount, filteredArmor);
    }
    
    private readonly Dictionary<string, IEnumerable<Func<Armor, object>>> sortSelectorsMap = new()
    {
        { SortArmorOption.Name, [(s => s.Name)] },
        { SortArmorOption.Category, [(s => s.ArmorCategory), (s => s.Name)] },
        { SortArmorOption.AC, [(s => s.BaseArmorClass), (s => s.Name)] },
        { SortArmorOption.Value, [(s => s.Value!), (s => s.Name)] },
        { SortArmorOption.Weight, [(s => s.Weight!), (s => s.Name)] },
        { SortArmorOption.Rarity, [(s => s.Rarity == null), (s => s.Rarity!), (s => s.Name)] },
    };
}