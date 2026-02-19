using Api.Data;
using Api.Models.DTOs.Items;
using Api.Models.DTOs.ResponseDtos;
using Api.Models.Items;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

using static Api.Validation.AllowedValues.ValuesValidator;
using static Api.Services.Util.QueryUtil;
using Api.Services.Util;
using Api.Validation.AllowedValues;
using Api.Middlewares.ExceptionHandling;

namespace Api.Repositories.Implemented.Items;

public class WeaponRepository(AppDbContext context) : IWeaponRepository
{
    public async Task<Weapon> GetByIdAsync(int id) => 
        await context.Weapons.FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new Exception($"Weapon with id {id} could not be found");

    public async Task<ICollection<Weapon>> GetAllAsync() => await context.Weapons.ToListAsync();

    public async Task<Weapon> CreateAsync(Weapon entity)
    {
        await context.Weapons.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

        public async Task DeleteAsync(Weapon entity)
    {
        context.Weapons.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Weapon> UpdateAsync(Weapon updatedEntity)
    {
        context.Weapons.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task<(int, ICollection<Weapon>)> GetFilteredAsync(WeaponFilterDto filter, PaginationRequestDto pagination)
    {      
        var query = context.Weapons
            .AsQueryable()
            .WhereIf(filter.Name, w => w.Name.Contains(filter.Name!))
            .WhereIf(filter.Category, w => w.Categories.Any(c => filter.Category!.Contains(c)))
            .WhereIf(filter.Rarity, w => w.Rarity.Contains(filter.Rarity!)) 
            .WhereIf(filter.RequiresAttunement, w => w.RequiresAttunement == filter.RequiresAttunement)
            
            .WhereIf(filter.MinWeight, w => w.Weight >= filter.MinWeight)
            .WhereIf(filter.MaxWeight, w => w.Weight <= filter.MaxWeight)
            .WhereIf(filter.MinValue, w => w.Value >= filter.MinValue)
            .WhereIf(filter.MaxValue, w => w.Value <= filter.MaxValue)
           
            .WhereIf(filter.WeaponCategory, w => filter.WeaponCategory!.Contains(w.WeaponCategory))
            .WhereIf(filter.WeaponType, w => filter.WeaponType!.Contains(w.WeaponType))
            .WhereIf(filter.Slot, w => filter.Slot!.Contains(w.EquipSlot))
            .WhereIf(filter.Property, w => w.Properties.Any(p => filter.Property!.Contains(p)))
            .WhereIf(filter.DamageType, w => w.DamageTypes.Any(d => filter.DamageType!.Contains(d)))

            .WhereIf(filter.MinRange, w => w.Value >= filter.MinRange)
            .WhereIf(filter.MaxRange, w => w.Value <= filter.MaxRange)
            .WhereIf(filter.LongRange, w => w.LongRange.HasValue == filter.LongRange)

            .WhereIf(filter.IsHomebrew, w => w.IsHomebrew == filter.IsHomebrew)
            .WhereIf(filter.CloningAllowed, w => w.CloningAllowed == filter.CloningAllowed);

        if (filter.SortBy is not null)
            query = SortBy(query, filter.SortBy, filter.SortDescending);
        
        var weaponCount = await query.CountAsync();
        var filteredWeapons = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (weaponCount, filteredWeapons);
    }

    public IQueryable<Weapon> SortBy(IQueryable<Weapon> query, string sortFilter, bool descending = false)
    {
        if(!TryNormalizeValue<SortWeaponOption>(sortFilter, out string? normalized))
            return context.Weapons;

        return normalized switch
        {
            SortWeaponOption.Name => OrderByMany(query, [(i => i.Name)], descending),
            SortWeaponOption.Category => OrderByMany(query, [(i => i.WeaponCategory), (i => i.Name)], descending),
            SortWeaponOption.Type => OrderByMany(query, [(i => i.WeaponType), (i => i.Name)], descending),
            SortWeaponOption.Value => OrderByMany(query, [(i => i.Value!), (i => i.Name)], descending),
            SortWeaponOption.Weight => OrderByMany(query, [(i => i.Weight!), (i => i.Name)], descending),
            SortWeaponOption.Rarity => OrderByMany(query, [(i => i.Rarity == null), (i => i.Rarity!), (i => i.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }
}