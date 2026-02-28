using System.Linq.Expressions;
using Api.Domain.Characters.DTOs;
using Api.Domain.Characters.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums;
using Api.Domain.Shared.Enums.Character;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Api.Domain.Shared.Utils.QueryExtensions;
using static Api.Infrastructure.Validation.ValuesValidator;

namespace Api.Domain.Characters.Repositories;

public class CharacterRepository(AppDbContext context) : ICharacterRepository
{
    public async Task<Character> GetByIdAsync(int id) =>
        await context.Characters.FindAsync(id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithInventoryAsync(int id) =>
        await context.Characters
            .Include(c => c.Inventory)
                .ThenInclude(i => i.StoredItems)
                .ThenInclude(i => i.Item)
            .Include(c => c.Inventory)
                .ThenInclude(i => i.EquipmentSlots)
                .ThenInclude(i => i.Equipment)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<CharacterInfo> GetCharacterInfoAsync(int id) =>
        await context.Characters
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(r => new CharacterInfo
            {
                AlignmentId = r.Info.AlignmentId,
                PersonalityTraits = r.Info.PersonalityTraits,
                Ideals = r.Info.Ideals,
                Bonds = r.Info.Bonds,
                Flaws = r.Info.Flaws,
                Age = r.Info.Age,
                Height = r.Info.Height,
                Weight = r.Info.Weight,
                Eyes = r.Info.Eyes,
                Skin = r.Info.Skin,
                Hair = r.Info.Hair,
                AlliesAndOrganizations = r.Info.AlliesAndOrganizations,
                Backstory = r.Info.Backstory,
                CharacterPictureUrl = r.Info.CharacterPictureUrl!
            })
            .FirstOrDefaultAsync()
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithCombatStatsAsync(int id) =>
        await context.Characters
            .Include(c => c.CombatStats)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithCharacterInfoAsync(int id) =>
        await context.Characters
            .Include(c => c.Info)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithClassesAsync(int id) =>
        await context.Characters
            .Include(c => c.Class)
            .Include(c => c.SubClass)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithFeaturesAsync(int id) =>
        await context.Characters
            .Include(c => c.Class)
                .ThenInclude(c => c.ClassLevels)
                    .ThenInclude(l => l.NewFeatures)
            .Include(c => c.SubClass)
                .ThenInclude(s => s!.ClassLevels)
                    .ThenInclude(l => l.NewFeatures)
            .Include(c => c.Race)
                    .ThenInclude(r => r.Traits)
            .Include(c => c.Subrace)
                .ThenInclude(s => s!.Traits)
            .Include(c => c.Background)
                .ThenInclude(b => b!.Features)
            .Include(c => c.Feats)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");


    public async Task<Character> GetWithAllDataAsync(int id) =>
        await context.Characters
            .Include(f => f.Class)
            .Include(f => f.SubClass)
            .Include(f => f.Background)
            .Include(f => f.Race)
            .Include(f => f.Subrace)
            .Include(c => c.Inventory)
            .Include(c => c.CombatStats)
            .Include(c => c.Info)
            .AsSplitQuery()
            .Include(c => c.CurrentClassSlots)
            .Include(c => c.AbilityScores)
            .Include(c => c.SavingThrows)
            .Include(c => c.DamageAffinities)
            .Include(c => c.SkillProficiencies)
            .Include(c => c.WeaponCategoryProficiencies)
            .Include(c => c.WeaponTypeProficiencies)
            .Include(c => c.ArmorProficiencies)
            .Include(c => c.ToolProficiencies)
            .Include(c => c.Languages)
            .Include(f => f.ReadySpells)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<ICollection<Character>> GetAllAsync() => await context.Characters.ToListAsync();

    public async Task<Character> CreateAsync(Character entity)
    {
        await context.Characters.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Character entity)
    {
        context.Characters.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task<Character> UpdateAsync(Character updatedEntity)
    {
        context.Characters.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

	public async Task<ICollection<Character>> GetAllAsync(CharacterFilterDto? filter = null, PaginationRequestDto? pagination = null, Guid? currentUserId = null)
	{
		var query = context.Characters.AsQueryable();

		if (filter is not null)
		{
			filter.Size = NormalizeValue<CreatureSize>(filter.Size);
			filter.Alignment = NormalizeValue<AlignmentType>(filter.Alignment);

			query = query
				.WhereIf(filter.Name, c => c.Name.Contains(filter.Name!))
				.WhereIf(filter.Class, c => filter.Class!.Contains(c.ClassId))
				.WhereIf(filter.Race, c => filter.Race!.Contains(c.RaceId))
				.WhereIf(filter.Subrace, c => filter.Subrace!.Contains(c.SubraceId ?? 0))
				.WhereIf(filter.Background, c => filter.Background!.Contains(c.BackgroundId))
				.WhereIf(filter.Alignment, c => filter.Alignment.Contains(c.Info.AlignmentName ?? ""))
				.WhereIf(filter.Size, c => filter.Size.Contains(c.Race.Size))

				.WhereIf(filter.MinLevel, c => c.Level >= filter.MinLevel)
				.WhereIf(filter.MaxLevel, c => c.Level <= filter.MaxLevel)

				.WhereIf(filter.CreatedBy, c => c.CreatedBy == filter.CreatedBy)
				.WhereIf(filter.IsHomebrew, c => c.IsHomebrew == filter.IsHomebrew)
				.WhereIf(filter.CloningAllowed, c => c.CloningAllowed == filter.CloningAllowed)
				.Where(c => c.IsPublic || c.CreatedBy == currentUserId);
		}

		var sortBy = NormalizeValue<SortCharacterOption>(filter?.SortBy ?? SortCharacterOption.Default);
		query = query.OrderByMany(sortSelectorsMap[sortBy], filter?.SortDescending ?? true);

		if(pagination is null)
			return await query.ToListAsync();

		var filteredCharacters = await query
			.Skip((pagination.Page - 1) * pagination.PageSize)
			.Take(pagination.PageSize)
			.ToListAsync();

		return filteredCharacters;
	}

	private readonly Dictionary<string, IEnumerable<Expression<Func<Character, object>>>> sortSelectorsMap = new()
    {
        { SortCharacterOption.Name, [(c => c.Name)] },
        { SortCharacterOption.Level, [(c => c.Level), (c => c.Name)] },
        { SortCharacterOption.TimeCreated, [(c => c.CreatedAt), (c => c.Name), (c => c.Level)] },
    };
}