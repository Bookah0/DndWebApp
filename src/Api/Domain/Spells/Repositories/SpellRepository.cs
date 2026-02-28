using static Api.Domain.Shared.Utils.QueryExtensions;
using static Api.Infrastructure.Validation.ValuesValidator;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Spells.DTOs;
using Api.Domain.Spells.Models;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Api.Domain.Shared.Utils;
using Api.Domain.Shared.Enums.Spells;
using Api.Domain.Shared.Enums.Damage;
using System.Linq.Expressions;

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
		await context.Spells.AddAsync(entity);
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

	public async Task<ICollection<Spell>> GetAllAsync(SpellFilterDto? filter = null, PaginationRequestDto? pagination = null)
	{
		var query = context.Spells.AsQueryable();

		if (filter is not null)
		{
			filter.MagicSchool = NormalizeValue<MagicSchool>(filter.MagicSchool);
			filter.Duration = NormalizeValue<SpellDuration>(filter.Duration);
			filter.CastingTime = NormalizeValue<CastingTime>(filter.CastingTime);
			filter.TargetType = NormalizeValue<TargetType>(filter.TargetType);
			filter.Range = NormalizeValue<SpellRange>(filter.Range);
			filter.SpellType = NormalizeValue<SpellType>(filter.SpellType);
			filter.DamageType = NormalizeValue<DamageType>(filter.DamageType);

			query = query
				.WhereIf(filter.Name, s => s.Name.Contains(filter.Name!))
				.WhereIf(filter.MagicSchool, s => filter.MagicSchool.Contains(s.MagicSchool))
				.WhereIf(filter.Duration, s => filter.Duration.Contains(s.Duration))
				.WhereIf(filter.CastingTime, s => filter.CastingTime.Contains(s.CastingTime))
				.WhereIf(filter.TargetType, s => filter.TargetType.Contains(s.SpellTargeting.TargetType))
				.WhereIf(filter.Range, s => filter.Range.Contains(s.SpellTargeting.Range))

				.WhereIf(filter.ClassId, s => s.Classes.Any(c => filter.ClassId!.Contains(c.Id)))
				.WhereIf(filter.SpellType, s => s.SpellTypes.Any(t => filter.SpellType.Contains(t)))
				.WhereIf(filter.DamageType, s => s.DamageTypes.Any(t => filter.DamageType.Contains(t)))

				.WhereIf(filter.MinLevel, s => s.Level >= filter.MinLevel)
				.WhereIf(filter.MaxLevel, s => s.Level <= filter.MaxLevel)
				.WhereIf(filter.MinRangeValue, s => s.SpellTargeting.RangeValue >= filter.MinRangeValue)
				.WhereIf(filter.MaxRangeValue, s => s.SpellTargeting.RangeValue <= filter.MaxRangeValue)

				.WhereIf(filter.CreatedBy, s => s.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, s => s.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, s => s.CloningAllowed == filter.CloningAllowed);
		}
		
		var sortBy = NormalizeValue<SortSpellOption>(filter?.SortBy ?? SortSpellOption.Default);
		query = query.OrderByMany(sortSelectorsMap[sortBy], filter?.SortDescending ?? false);
		
		if(pagination is null)
			return await query.ToListAsync();
			
		var filteredSpells = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return filteredSpells;
	}

	private readonly Dictionary<string, IEnumerable<Expression<Func<Spell, object>>>> sortSelectorsMap = new()
	{
		{ SortSpellOption.Name, [(s => s.Name)] },
		{ SortSpellOption.Level, [(s => s.Level), (s => s.Name)] },
		{ SortSpellOption.CastingTime, [(s => s.CastingTime), (s => s.CastingTimeValue ?? 0), (s => s.Name)] },
		{ SortSpellOption.Duration, [(s => s.Duration), (s => s.DurationValue ?? 0), (s => s.Name)] },
		{ SortSpellOption.Target, [(s => s.SpellTargeting.TargetType), (s => s.Name)] },
		{ SortSpellOption.Range, [(s => s.SpellTargeting.Range), (s => s.SpellTargeting.RangeValue ?? 0), (s => s.Name)] },
	};
}