using static Api.Domain.Shared.Utils.QueryExtensions;
using static Api.Infrastructure.Validation.ValuesValidator;
using Api.Domain.Items.Models;
using Api.Infrastructure.Data;
using Api.Domain.Items.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Shared.Utils;
using Api.Domain.Shared.Enums.Items;
using Api.Domain.Shared.Enums.Damage;
using System.Linq.Expressions;

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

	public async Task<ICollection<Weapon>> GetAllAsync(WeaponFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Weapons.AsQueryable();

		if (filter is not null)
		{
			filter.Rarity = NormalizeValue<ItemRarity>(filter.Rarity);
			filter.WeaponCategory = NormalizeValue<WeaponCategory>(filter.WeaponCategory);
			filter.WeaponType = NormalizeValue<WeaponType>(filter.WeaponType);
			filter.Slot = NormalizeValue<EquipSlot>(filter.Slot);
			filter.Property = NormalizeValue<WeaponProperty>(filter.Property);
			filter.DamageType = NormalizeValue<DamageType>(filter.DamageType);

			query = query
				.WhereIf(filter.Name, w => w.Name.Contains(filter.Name!))
				.WhereIf(filter.Rarity, w => filter.Rarity.Contains(w.Rarity))
				.WhereIf(filter.RequiresAttunement, w => w.RequiresAttunement == filter.RequiresAttunement)

				.WhereIf(filter.WeaponCategory, w => filter.WeaponCategory.Contains(w.WeaponCategory))
				.WhereIf(filter.WeaponType, w => filter.WeaponType.Contains(w.WeaponType))
				.WhereIf(filter.Slot, w => filter.Slot.Contains(w.EquipSlot!))
				.WhereIf(filter.Property, w => w.Properties.Any(p => filter.Property.Contains(p)))
				.WhereIf(filter.DamageType, w => w.DamageTypes.Any(d => filter.DamageType.Contains(d)))

				.WhereIf(filter.MinWeight, w => w.Weight >= filter.MinWeight)
				.WhereIf(filter.MaxWeight, w => w.Weight <= filter.MaxWeight)
				.WhereIf(filter.MinValue, w => w.Value >= filter.MinValue)
				.WhereIf(filter.MaxValue, w => w.Value <= filter.MaxValue)
				.WhereIf(filter.MinRange, w => w.Value >= filter.MinRange)
				.WhereIf(filter.MaxRange, w => w.Value <= filter.MaxRange)
				.WhereIf(filter.LongRange, w => w.LongRange.HasValue == filter.LongRange)

				.WhereIf(filter.CreatedBy, i => i.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, w => w.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, w => w.CloningAllowed == filter.CloningAllowed);
		}

		var sortBy = NormalizeValue<SortToolOption>(filter?.SortBy ?? SortToolOption.Default);
		query = query.OrderByMany(sortSelectorsMap[sortBy], filter?.SortDescending ?? false);

		if(pagination is null)
			return await query.ToListAsync();
			
		var filteredWeapons = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return filteredWeapons;
	}

	private readonly Dictionary<string, IEnumerable<Expression<Func<Weapon, object>>>> sortSelectorsMap = new()
	{
		{ SortWeaponOption.Name, [(s => s.Name)] },
		{ SortWeaponOption.Category, [(s => s.WeaponCategory), (s => s.Name)] },
		{ SortWeaponOption.Type, [(s => s.WeaponType), (s => s.Name)] },
		{ SortWeaponOption.Value, [(s => s.Value ?? 0), (s => s.Name)] },
		{ SortWeaponOption.Weight, [(s => s.Weight ?? 0), (s => s.Name)] },
		{ SortWeaponOption.Rarity, [(s => s.Rarity == null), (s => s.Rarity), (s => s.Name)] },
	};
}