using static Api.Domain.Shared.Utils.QueryUtil;
using Api.Domain.Items.Models;
using Api.Infrastructure.Data;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Validation;
using Api.Infrastructure.Middleware.ExceptionHandling;
using Api.Domain.Shared.Utils;

namespace Api.Domain.Items.Repositories;

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
        var normalizedSortBy = ValuesValidator.NormalizeValueOrThrow<SortWeaponOption>(filter.SortBy ?? SortWeaponOption.Default);

        var query = context.Weapons
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
           
            .WhereIf(filter.WeaponCategory, w => filter.WeaponCategory!.Contains(w.WeaponCategory))
            .WhereIf(filter.WeaponType, w => filter.WeaponType!.Contains(w.WeaponType))
            .WhereIf(filter.Slot, w => filter.Slot!.Contains(w.EquipSlot!))
            .WhereIf(filter.Property, w => w.Properties.Any(p => filter.Property!.Contains(p)))
            .WhereIf(filter.DamageType, w => w.DamageTypes.Any(d => filter.DamageType!.Contains(d)))

            .WhereIf(filter.MinRange, w => w.Value >= filter.MinRange)
            .WhereIf(filter.MaxRange, w => w.Value <= filter.MaxRange)
            .WhereIf(filter.LongRange, w => w.LongRange.HasValue == filter.LongRange)

            .WhereIf(filter.IsHomebrew, w => w.IsHomebrew == filter.IsHomebrew)
            .WhereIf(filter.CloningAllowed, w => w.CloningAllowed == filter.CloningAllowed)
            .SortBy(normalizedSortBy, sortSelectorsMap, SortWeaponOption.Default, filter.SortDescending); 
        
        var weaponCount = await query.CountAsync();
        var filteredWeapons = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return (weaponCount, filteredWeapons);
    }

    private readonly Dictionary<string, IEnumerable<Func<Weapon, object>>> sortSelectorsMap = new()
    {
        { SortWeaponOption.Name, [(s => s.Name)] },
        { SortWeaponOption.Category, [(s => s.WeaponCategory), (s => s.Name)] },
        { SortWeaponOption.Type, [(s => s.WeaponType), (s => s.Name)] },
        { SortWeaponOption.Value, [(s => s.Value!), (s => s.Name)] },
        { SortWeaponOption.Weight, [(s => s.Weight!), (s => s.Name)] },
        { SortWeaponOption.Rarity, [(s => s.Rarity == null), (s => s.Rarity!), (s => s.Name)] },
    };
}