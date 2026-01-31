
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Services.Util;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Repositories.Implemented.Spells;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;
using static DndWebApp.Api.Services.Util.ConstantsUtil;
using static DndWebApp.Api.Services.Util.ValidationUtil;
using DndWebApp.Api.Models.DTOs.Spells;
using DndWebApp.Api.Services.Constants;
using DndWebApp.Api.Models.Spells.Constants;
using DndWebApp.Api.Models.Items.Constants;

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
        HasContentOrThrow(dto.Name);
        HasContentOrThrow(dto.Description);
        HasContentOrThrow(dto.Duration);
        HasContentOrThrow(dto.CastingTime);
        HasContentOrThrow(dto.MagicSchool);

        var dtoSchool = ResolveOptionOrThrow(dto.MagicSchool, MagicSchool.AllowedValues, "Magic School");
        var dtoTargetType = ResolveOptionOrThrow(dto.TargetingDto.TargetType, SpellTargetType.AllowedValues, "Spell Target Type");
        var dtoSpellRange = ResolveOptionOrThrow(dto.TargetingDto.Range, SpellRange.AllowedValues, "Spell Range");
        var dtoDuration = ResolveOptionOrThrow(dto.Duration, SpellDuration.AllowedValues, "Spell Duration");
        var dtoCastTime = ResolveOptionOrThrow(dto.CastingTime, CastingTime.AllowedValues, "Casting Time");
        var dtoSpellTypes = ResolveOptionOrThrow(dto.Types, SpellType.AllowedValues, "Spell Type");
        var dtoDamageTypes = ResolveOptionOrThrow(dto.DamageTypes, DamageType.AllowedValues, "Damage Type");

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
            SpellTypes = dtoSpellTypes!,
            DamageRoll = dto.DamageRoll,
            DamageTypes = dtoDamageTypes!,
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

        await IdsExist<IClassRepository, Class>(dto.ClassIds, classRepo);

        var dtoSchools = dto.MagicSchools != null ? ResolveOptionOrThrow(dto.MagicSchools, MagicSchool.AllowedValues, "Magic School") : null;
        var dtoTargetTypes = dto.TargetTypes != null ? ResolveOptionOrThrow(dto.TargetTypes, SpellTargetType.AllowedValues, "Spell Target Type") : null;
        var dtoSpellRanges = dto.Range != null ? ResolveOptionOrThrow(dto.Range, SpellRange.AllowedValues, "Spell Range") : null;
        var dtoDurations = dto.Durations != null ? ResolveOptionOrThrow(dto.Durations, SpellDuration.AllowedValues, "Spell Duration") : null;
        var dtoCastTimes = dto.CastingTimes != null ? ResolveOptionOrThrow(dto.CastingTimes, CastingTime.AllowedValues, "Casting Time") : null;
        var dtoSpellTypes = dto.SpellTypes != null ? ResolveOptionOrThrow(dto.SpellTypes, SpellType.AllowedValues, "Spell Type") : null;
        var dtoDamageTypes = dto.DamageTypes != null ? ResolveOptionOrThrow(dto.DamageTypes, DamageType.AllowedValues, "Damage Type") : null;

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

    public async Task UpdateAsync(int id, SpellDto dto)
    {
        var spell = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Spell could not be found");

        HasContentOrThrow(dto.Name);
        HasContentOrThrow(dto.Description);
        HasContentOrThrow(dto.Duration);
        HasContentOrThrow(dto.CastingTime);
        HasContentOrThrow(dto.MagicSchool);

        var dtoSchool = ResolveOptionOrThrow(dto.MagicSchool, MagicSchool.AllowedValues, "Magic School");
        var dtoTargetType = ResolveOptionOrThrow(dto.TargetingDto.TargetType, SpellTargetType.AllowedValues, "Spell Target Type");
        var dtoSpellRange = ResolveOptionOrThrow(dto.TargetingDto.Range, SpellRange.AllowedValues, "Spell Range");
        var dtoDuration = ResolveOptionOrThrow(dto.Duration, SpellDuration.AllowedValues, "Spell Duration");
        var dtoCastTime = ResolveOptionOrThrow(dto.CastingTime, CastingTime.AllowedValues, "Casting Time");
        var dtoSpellTypes = ResolveOptionOrThrow(dto.Types, SpellType.AllowedValues, "Spell Type");
        var dtoDamageTypes = ResolveOptionOrThrow(dto.DamageTypes, DamageType.AllowedValues, "Damage Type");

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
        spell.SpellTypes = dtoSpellTypes!;

        spell.DamageRoll = dto.DamageRoll;
        spell.DamageTypes = dtoDamageTypes!;

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

    public ICollection<Spell> SortBy(ICollection<Spell> spells, string sortFilter, bool descending = false)
    {
        if(!TryResolveOption(sortFilter, SortSpellOption.AllowedValues, out string? resolved))
            return spells;

        return resolved switch
        {
            SortSpellOption.Name => OrderByMany(spells, [(s => s.Name)], descending),
            SortSpellOption.Level => OrderByMany(spells, [(s => s.Level), (s => s.Name)], descending),
            SortSpellOption.CastingTime => OrderByMany(spells, [(s => s.CastingTime), (s => s.CastingTimeValue), (s => s.Name)], descending),
            SortSpellOption.Duration => OrderByMany(spells, [(s => s.Duration), (s => s.DurationValue), (s => s.Name)], descending),
            SortSpellOption.Target => OrderByMany(spells, [(s => s.SpellTargeting.TargetType), (s => s.Name)], descending),
            SortSpellOption.Range => OrderByMany(spells, [(s => s.SpellTargeting.Range), (s => s.SpellTargeting.RangeValue), (s => s.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}