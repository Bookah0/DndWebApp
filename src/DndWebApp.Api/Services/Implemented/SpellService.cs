using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Repositories.Implemented.Spells;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Implemented;

public class SpellService : ISpellService
{
    private readonly ISpellRepository repo;
    private readonly IClassRepository classRepo;
    private readonly ILogger<SpellService> logger;

    public SpellService(ISpellRepository repo, IClassRepository classRepo, ILogger<SpellService> logger)
    {
        this.repo = repo;
        this.classRepo = classRepo;
        this.logger = logger;
    }

    public async Task<Spell> CreateAsync(SpellDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.Duration);
        ValidationUtil.HasContentOrThrow(dto.CastingTime);
        ValidationUtil.HasContentOrThrow(dto.MagicSchool);

        var dtoSchool = NormalizationUtil.ParseEnumOrThrow<MagicSchool>(dto.MagicSchool);
        var dtoTargetType = NormalizationUtil.ParseEnumOrThrow<SpellTargetType>(dto.TargetingDto.TargetType);
        var dtoSpellRange = NormalizationUtil.ParseEnumOrThrow<SpellRange>(dto.TargetingDto.Range);
        var dtoDuration = NormalizationUtil.ParseEnumOrThrow<SpellDuration>(dto.Duration);
        var dtoCastTime = NormalizationUtil.ParseEnumOrThrow<CastingTime>(dto.CastingTime);
        var dtoSpellTypes = NormalizationUtil.ParseEnumOrThrow<SpellType>(dto.Types);
        var dtoDamageTypes = NormalizationUtil.ParseEnumOrThrow<DamageType>(dto.DamageTypes);

        if (dto.Level <= 0)
            throw new ArgumentOutOfRangeException($"Spell level is set to {dto.Level}. It must be greater than 0");
        if (dto.TargetingDto.RangeValue > 0 && dtoSpellRange != SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.TargetingDto.RangeValue} but spell is not of range type SpellRange.Feet.");
        if (dto.TargetingDto.RangeValue % 5 != 0 && dtoSpellRange == SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.TargetingDto.RangeValue}. It must be 5*n (feet).");

        var spell = new Spell()
        {
            Name = dto.Name,
            Description = dto.Description,
            IsHomebrew = dto.IsHomebrew,
            Level = dto.Level,
            EffectsAtHigherLevels = dto.EffectsAtHigherLevels,
            Duration = dtoDuration,
            CastingTime = dtoCastTime,
            ReactionCondition = dto.ReactionCondition,
            MagicSchool = dtoSchool,
            SpellTypes = dtoSpellTypes,
            DamageRoll = dto.DamageRoll,
            DamageTypes = dtoDamageTypes,
            SpellTargeting = new SpellTargeting()
            {
                TargetType = dtoTargetType,
                Range = dtoSpellRange,
                RangeValue = dto.TargetingDto.RangeValue,
                ShapeLength = dto.TargetingDto.ShapeLength,
                ShapeType = dto.TargetingDto.ShapeType,
                ShapeWidth = dto.TargetingDto.ShapeWidth
            },
            CastingRequirements = new CastingRequirements
            {
                Verbal = dto.CastRequirementsDto.Verbal,
                Somatic = dto.CastRequirementsDto.Somatic,
                Materials = dto.CastRequirementsDto.Materials,
                MaterialCost = dto.CastRequirementsDto.MaterialCost,
                MaterialsConsumed = dto.CastRequirementsDto.MaterialsConsumed
            }
        };

        return await repo.CreateAsync(spell);
    }


    public async Task DeleteAsync(int id)
    {
        var spell = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Spell could not be found");
        await repo.DeleteAsync(spell);
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

        await ValidationUtil.IdsExist<IClassRepository, Class>(dto.ClassIds, classRepo);

        var dtoSchools = NormalizationUtil.ParseEnumOrThrow<MagicSchool>(dto.MagicSchools);
        var dtoTargetTypes = NormalizationUtil.ParseEnumOrThrow<SpellTargetType>(dto.TargetTypes);
        var dtoSpellRanges = NormalizationUtil.ParseEnumOrThrow<SpellRange>(dto.Range);
        var dtoDurations = NormalizationUtil.ParseEnumOrThrow<SpellDuration>(dto.Durations);
        var dtoCastTimes = NormalizationUtil.ParseEnumOrThrow<CastingTime>(dto.CastingTimes);
        var dtoSpellTypes = NormalizationUtil.ParseEnumOrThrow<SpellType>(dto.SpellTypes);
        var dtoDamageTypes = NormalizationUtil.ParseEnumOrThrow<DamageType>(dto.DamageTypes);

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
            throw new ArgumentOutOfRangeException(nameof(filter.MaxLevel), "Maximum level must be greater than or equal to minimum level");
        if (filter.Name is not null)
            filter.Name = NormalizationUtil.NormalizeWhiteSpace(filter.Name);

        return await repo.FilterAllAsync(filter);
    }

    public async Task<Spell> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Spell could not be found");
    }

    public async Task UpdateAsync(SpellDto dto)
    {
        var spell = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Spell could not be found");

        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.HasContentOrThrow(dto.Description);
        ValidationUtil.HasContentOrThrow(dto.Duration);
        ValidationUtil.HasContentOrThrow(dto.CastingTime);
        ValidationUtil.HasContentOrThrow(dto.MagicSchool);

        var dtoSchool = NormalizationUtil.ParseEnumOrThrow<MagicSchool>(dto.MagicSchool);
        var dtoTargetType = NormalizationUtil.ParseEnumOrThrow<SpellTargetType>(dto.TargetingDto.TargetType);
        var dtoSpellRange = NormalizationUtil.ParseEnumOrThrow<SpellRange>(dto.TargetingDto.Range);
        var dtoDuration = NormalizationUtil.ParseEnumOrThrow<SpellDuration>(dto.Duration);
        var dtoCastTime = NormalizationUtil.ParseEnumOrThrow<CastingTime>(dto.CastingTime);
        var dtoSpellTypes = NormalizationUtil.ParseEnumOrThrow<SpellType>(dto.Types);
        var dtoDamageTypes = NormalizationUtil.ParseEnumOrThrow<DamageType>(dto.DamageTypes);

        if (dto.Level <= 0)
            throw new ArgumentOutOfRangeException($"Spell level is set to {dto.Level}. It must be greater than 0");
        if (dto.TargetingDto.RangeValue > 0 && dtoSpellRange != SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.TargetingDto.RangeValue} but spell is not of range type SpellRange.Feet.");
        if (dto.TargetingDto.RangeValue % 5 != 0 && dtoSpellRange == SpellRange.Feet)
            throw new ArgumentOutOfRangeException($"Range value is set to {dto.TargetingDto.RangeValue}. It must be 5*n (feet).");

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

        spell.DamageRoll = dto.DamageRoll;
        spell.DamageTypes = dtoDamageTypes;

        spell.SpellTargeting.TargetType = dtoTargetType;
        spell.SpellTargeting.Range = dtoSpellRange;
        spell.SpellTargeting.RangeValue = dto.TargetingDto.RangeValue;
        spell.SpellTargeting.ShapeLength = dto.TargetingDto.ShapeLength;
        spell.SpellTargeting.ShapeType = dto.TargetingDto.ShapeType;
        spell.SpellTargeting.ShapeWidth = dto.TargetingDto.ShapeWidth;

        spell.CastingRequirements.Verbal = dto.CastRequirementsDto.Verbal;
        spell.CastingRequirements.Somatic = dto.CastRequirementsDto.Somatic;
        spell.CastingRequirements.Materials = dto.CastRequirementsDto.Materials;
        spell.CastingRequirements.MaterialCost = dto.CastRequirementsDto.MaterialCost;
        spell.CastingRequirements.MaterialsConsumed = dto.CastRequirementsDto.MaterialsConsumed;

        await repo.UpdateAsync(spell);
    }

    
    public ICollection<Spell> SortBy(ICollection<Spell> spells, SpellSortFilter sortFilter, bool descending = false)
    {
        return sortFilter switch
        {
            SpellSortFilter.Name => SortUtil.OrderByMany(spells, [(s => s.Name)], descending),
            SpellSortFilter.Level => SortUtil.OrderByMany(spells, [(s => s.Level), (s => s.Name)], descending),
            SpellSortFilter.CastingTime => SortUtil.OrderByMany(spells, [(s => s.CastingTime), (s => s.CastingTimeValue), (s => s.Name)], descending),
            SpellSortFilter.Duration => SortUtil.OrderByMany(spells, [(s => s.Duration), (s => s.DurationValue), (s => s.Name)], descending),
            SpellSortFilter.Target => SortUtil.OrderByMany(spells, [(s => s.SpellTargeting.TargetType), (s => s.Name)], descending),
            SpellSortFilter.Range => SortUtil.OrderByMany(spells, [(s => s.SpellTargeting.Range), (s => s.SpellTargeting.RangeValue), (s => s.Name)], descending),
            _ => spells,
        };
    }
}