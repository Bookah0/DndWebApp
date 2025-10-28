using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Repositories.Classes;
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Services.Spells;

public class SpellService : IService<Spell, SpellDto, SpellDto>
{
    protected SpellRepository repo;
    protected AppDbContext context;
    protected ClassRepository classRepo;

    public SpellService(SpellRepository repo, ClassRepository classRepo, AppDbContext context)
    {
        this.context = context;
        this.repo = repo;
        this.classRepo = classRepo;
    }

    public async Task<Spell> CreateAsync(SpellDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.Duration);
        ValidationUtil.ValidateRequiredString(dto.CastingTime);
        ValidationUtil.ValidateRequiredString(dto.MagicSchool);

        var dtoSchool = ValidationUtil.ParseEnumOrThrow<MagicSchool>(dto.MagicSchool);
        var dtoTargetType = ValidationUtil.ParseEnumOrThrow<SpellTargetType>(dto.TargetType);
        var dtoSpellRange = ValidationUtil.ParseEnumOrThrow<SpellRange>(dto.Range);
        var dtoDuration = ValidationUtil.ParseEnumOrThrow<SpellDuration>(dto.Duration);
        var dtoCastTime = ValidationUtil.ParseEnumOrThrow<CastingTime>(dto.CastingTime);
        var dtoSpellTypes = ValidationUtil.ParseEnumOrThrow<SpellType>(dto.Types);
        var dtoDamageTypes = ValidationUtil.ParseEnumOrThrow<DamageType>(dto.DamageTypes);

        if (dto.Level <= 0)
            throw new ArgumentOutOfRangeException($"Spell level is set to {dto.Level}. It must be greater than 0");
        if (dto.RangeValue > 0 && dtoSpellRange != SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.RangeValue} but spell is not of range type SpellRange.Feet.");
        if (dto.RangeValue % 5 != 0 && dtoSpellRange == SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.RangeValue}. It must be 5*n (feet).");

        var factorySpell = SpellFactory.Create(
            dto.Name,
            dto.Description,
            dto.IsHomebrew,
            dto.Level,
            dto.EffectsAtHigherLevels,
            dto.ReactionCondition,
            SpellFactory.CreateTargeting(dtoTargetType, dtoSpellRange, dto.RangeValue, dto.ShapeType, dto.ShapeWidth, dto.ShapeLength),
            dto.DamageRoll,
            dtoDamageTypes,
            SpellFactory.CreateCastingRequirments(dto.Verbal, dto.Somatic, dto.Materials, dto.MaterialCost, dto.MaterialsConsumed),
            dtoDuration,
            dtoCastTime,
            dtoSchool,
            dtoSpellTypes
        );

        await repo.CreateAsync(factorySpell);
        await context.SaveChangesAsync();
        return factorySpell;
    }

    public enum SpellSorting { Name, Level, CastingTime, Duration, Target, Range }
    public ICollection<Spell> SortBy(ICollection<Spell> spells, SpellSorting sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            SpellSorting.Name => SortUtil.OrderByMany(spells, [(s => s.Name)], descending),
            SpellSorting.Level => SortUtil.OrderByMany(spells, [(s => s.Level), (s => s.Name)], descending),
            SpellSorting.CastingTime => SortUtil.OrderByMany(spells, [(s => s.CastingTime), (s => s.CastingTimeValue), (s => s.Name)], descending),
            SpellSorting.Duration => SortUtil.OrderByMany(spells, [(s => s.Duration), (s => s.DurationValue), (s => s.Name)], descending),
            SpellSorting.Target => SortUtil.OrderByMany(spells, [(s => s.SpellTargeting.TargetType), (s => s.Name)], descending),
            SpellSorting.Range => SortUtil.OrderByMany(spells, [(s => s.SpellTargeting.Range), (s => s.SpellTargeting.RangeValue), (s => s.Name)], descending),
            _ => spells,
        };
    }

    public async Task DeleteAsync(int id)
    {
        var spell = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Spell could not be found");
        await repo.DeleteAsync(spell);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Spell>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<ICollection<Spell>> FilterAllAsync(SpellFilterDto dto)
    {
        if (dto.Name is not null)
            dto.Name = NormalizationUtil.NormalizeWhiteSpace(dto.Name);
        if (dto.MinLevel is not null && dto.MaxLevel is not null && dto.MinLevel > dto.MaxLevel)
            throw new ArgumentOutOfRangeException(nameof(dto), "Maximum level must be greater than or equal to minimum level");
        if (dto.MinLevel is not null && dto.MinLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(dto), "Minimum level must be greater than or equal to zero");
        if (dto.MaxLevel is not null && dto.MaxLevel < 0)
            throw new ArgumentOutOfRangeException(nameof(dto), "Maximum level must be greater than or equal to zero");

        await ValidationUtil.ValidateIdsExist<ClassRepository, Class>(dto.ClassIds, classRepo);

        var dtoSchools = ValidationUtil.ParseEnumOrThrow<MagicSchool>(dto.MagicSchools);
        var dtoTargetTypes = ValidationUtil.ParseEnumOrThrow<SpellTargetType>(dto.TargetTypes);
        var dtoSpellRanges = ValidationUtil.ParseEnumOrThrow<SpellRange>(dto.Range);
        var dtoDurations = ValidationUtil.ParseEnumOrThrow<SpellDuration>(dto.Durations);
        var dtoCastTimes = ValidationUtil.ParseEnumOrThrow<CastingTime>(dto.CastingTimes);
        var dtoSpellTypes = ValidationUtil.ParseEnumOrThrow<SpellType>(dto.SpellTypes);
        var dtoDamageTypes = ValidationUtil.ParseEnumOrThrow<DamageType>(dto.DamageTypes);

        var filter = new SpellFilter()
        {
            Name = dto.Name,
            MinLevel = dto.MinLevel,
            MaxLevel = dto.MaxLevel,
            IsHomebrew = dto.IsHomebrew,
            ClassIds = dto.ClassIds,
            Durations = dtoDurations,
            CastingTimes = dtoCastTimes,
            MagicSchools = dtoSchools,
            SpellTypes = dtoSpellTypes,
            TargetType = dtoTargetTypes,
            Range = dtoSpellRanges,
            DamageTypes = dtoDamageTypes,
        };

        if (filter.MinLevel > filter.MaxLevel)
            throw new ArgumentOutOfRangeException(nameof(filter), "Maximum level must be greater than or equal to minimum level");
        if (filter.Name is not null)
            filter.Name = NormalizationUtil.NormalizeWhiteSpace(filter.Name);

        return await repo.FilterAllAsync(filter);
    }

    public async Task<Spell> GetByIdAsync(int id)
    {
        var spell = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Spell could not be found");
        return spell;
    }

    public async Task UpdateAsync(SpellDto dto)
    {
        var spell = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Spell could not be found");

        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.Duration);
        ValidationUtil.ValidateRequiredString(dto.CastingTime);
        ValidationUtil.ValidateRequiredString(dto.MagicSchool);

        var dtoSchool = ValidationUtil.ParseEnumOrThrow<MagicSchool>(dto.MagicSchool);
        var dtoTargetType = ValidationUtil.ParseEnumOrThrow<SpellTargetType>(dto.TargetType);
        var dtoSpellRange = ValidationUtil.ParseEnumOrThrow<SpellRange>(dto.Range);
        var dtoDuration = ValidationUtil.ParseEnumOrThrow<SpellDuration>(dto.Duration);
        var dtoCastTime = ValidationUtil.ParseEnumOrThrow<CastingTime>(dto.CastingTime);
        var dtoSpellTypes = ValidationUtil.ParseEnumOrThrow<SpellType>(dto.Types);
        var dtoDamageTypes = ValidationUtil.ParseEnumOrThrow<DamageType>(dto.DamageTypes);

        if (dto.Level <= 0)
            throw new ArgumentOutOfRangeException($"Spell level is set to {dto.Level}. It must be greater than 0");
        if (dto.RangeValue > 0 && dtoSpellRange != SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.RangeValue} but spell is not of range type SpellRange.Feet.");
        if (dto.RangeValue % 5 != 0 && dtoSpellRange == SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.RangeValue}. It must be 5*n (feet).");

        spell.Name = dto.Name;
        spell.Description = dto.Description;
        spell.IsHomebrew = dto.IsHomebrew;
        spell.Level = dto.Level;
        spell.EffectsAtHigherLevels = dto.EffectsAtHigherLevels;
        spell.Duration = dtoDuration;
        spell.CastingTime = dtoCastTime;
        spell.ReactionCondition = dto.ReactionCondition;
        spell.MagicSchool = dtoSchool;
        spell.SpellTypes = dtoSpellTypes;

        spell.SpellTargeting = SpellFactory.CreateTargeting(dtoTargetType, dtoSpellRange, dto.RangeValue, dto.ShapeType, dto.ShapeWidth, dto.ShapeLength);
        spell.DamageRoll = dto.DamageRoll;
        spell.DamageTypes = dtoDamageTypes;
        spell.CastingRequirements = SpellFactory.CreateCastingRequirments(dto.Verbal, dto.Somatic, dto.Materials, dto.MaterialCost, dto.MaterialsConsumed);

        await repo.UpdateAsync(spell);
        await context.SaveChangesAsync();
    }
}