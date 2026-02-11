
using Api.Models.Spells;
using Api.Services.Util;
using Api.Repositories.Interfaces;
using Api.Repositories.Implemented.Spells;
using Api.Services.Interfaces;
using Api.Models.DTOs.Spells;
using Api.Middlewares.ExceptionHandling;

using static Api.Services.Util.SortUtil;
using static Api.Validation.AllowedValues.ValuesValidator;
using Api.Validation.AllowedValues.Spells;
using Api.Validation.AllowedValues;

namespace Api.Services.Implemented;

public class SpellService(
    ISpellRepository repo, 
    IBaseClassRepository classRepo, 
    ICurrentUserService currentUserService,
    ILogger<SpellService> logger) 
    : ISpellService
{
    public async Task<Spell> CreateAsync(CreateSpellRequestDto dto)
    {
        logger.LogInformation("Creating spell, Name: {SpellName}", dto.Name);

        var dtoTargetType = ResolveValueOrThrow<TargetType>(dto.TargetingDto.TargetType);
        var dtoSpellRange = ResolveValueOrThrow<SpellRange>(dto.TargetingDto.Range);
        var dtoDuration = ResolveValueOrThrow<SpellDuration>(dto.Duration);
        var dtoCastTime = ResolveValueOrThrow<CastingTime>(dto.CastingTime);

        if (dto.TargetingDto.RangeValue > 0 && dtoSpellRange != SpellRange.Feet && dtoSpellRange != SpellRange.Mile)
            throw new ValidationException($"Range value is set to {dto.TargetingDto.RangeValue} but spell is not of range type SpellRange.Feet or SpellRange.Mile.");
        if (dto.TargetingDto.RangeValue % 5 != 0 && dtoSpellRange == SpellRange.Feet)
            throw new ValidationException($"Range value is set to {dto.TargetingDto.RangeValue}. It must be 5*n (feet).");

        var spell = new Spell()
        {
            Name = dto.Name,
            Description = dto.Description,
            Level = dto.Level,
            EffectsAtHigherLevels = dto.EffectsAtHigherLevels,
            Duration = dtoDuration,
            DurationValue = dto.DurationValue,
            CastingTime = dtoCastTime,
            ReactionCondition = dto.ReactionCondition,
            MagicSchool = ResolveValueOrEmpty<MagicSchool>(dto.MagicSchool),
            SpellTypes = ResolveValueOrEmpty<SpellType>(dto.SpellTypes),
            DamageRoll = dto.DamageRoll,
            DamageTypes = ResolveValueOrEmpty<DamageType>(dto.DamageTypes),
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
            },
            CreatedAt = DateTime.UtcNow,
            CreatedBy = currentUserService.GetCurrentUserId(),
        };

        spell = await repo.CreateAsync(spell);
        logger.LogInformation("Successfully created spell, Name: {SpellName} ID {SpellId}", spell.Name, spell.Id);
        return spell;
    }

    public async Task DeleteAsync(int id)
    {
        var spell = await repo.GetByIdAsync(id);
    
        if(!spell.IsHomebrew)
            throw new ValidationException("Cannot delete a base spell.");

        logger.LogInformation("Deleting spell, ID: {SpellId}", id);
        var spellName = spell.Name;
        await repo.DeleteAsync(spell);

        logger.LogInformation("Successfully deleted spell, Name: {SpellName} ID: {SpellId}", spellName, id);
    }

    public async Task<ICollection<Spell>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Spell> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

    public async Task<Spell> UpdateAsync(int id, UpdateSpellRequestDto dto)
    {
        var spell = await repo.GetByIdAsync(id);

        if (dto.TargetingDto?.RangeValue > 0 && spell.SpellTargeting.Range != SpellRange.Feet)
            throw new ValidationException($"Range value is set to {dto.TargetingDto.RangeValue} but spell is not of range type SpellRange.Feet.");
        if (dto.TargetingDto?.RangeValue % 5 != 0 && spell.SpellTargeting.Range == SpellRange.Feet)
            throw new ValidationException($"Range value is set to {dto.TargetingDto!.RangeValue}. It must be 5*n (feet).");
        
        logger.LogInformation("Updating spell, Name: {SpellName} ID: {SpellId}", spell.Name, id);

        if(dto.MagicSchool is not null)
            spell.MagicSchool = ResolveValueOrEmpty<MagicSchool>(dto.MagicSchool);

        if(dto.TargetingDto?.TargetType is not null)
            spell.SpellTargeting.TargetType = ResolveValueOrEmpty<TargetType>(dto.TargetingDto.TargetType);

        if(dto.TargetingDto?.Range is not null)
            spell.SpellTargeting.Range = ResolveValueOrEmpty<SpellRange>(dto.TargetingDto.Range);

        if(dto.Duration is not null)
            spell.Duration = ResolveValueOrEmpty<SpellDuration>(dto.Duration);

        if(dto.CastingTime is not null)
            spell.CastingTime = ResolveValueOrEmpty<CastingTime>(dto.CastingTime);

        spell.Name = dto.Name ?? spell.Name;
        spell.Description = dto.Description ?? spell.Description;
        spell.Level = dto.Level ?? spell.Level;
        spell.EffectsAtHigherLevels = dto.EffectsAtHigherLevels ?? spell.EffectsAtHigherLevels;
        spell.ReactionCondition = dto.ReactionCondition ?? spell.ReactionCondition;
        spell.DurationValue = dto.DurationValue ?? spell.DurationValue;
        spell.CastingTimeValue = dto.CastingTimeValue ?? spell.CastingTimeValue;

        spell.DamageRoll = dto.DamageRoll ?? spell.DamageRoll;
        spell.SpellTargeting.RangeValue = dto.TargetingDto?.RangeValue ?? spell.SpellTargeting.RangeValue;
        spell.SpellTargeting.ShapeLength = dto.TargetingDto?.ShapeLength ?? spell.SpellTargeting.ShapeLength;
        spell.SpellTargeting.ShapeType = dto.TargetingDto?.ShapeType ?? spell.SpellTargeting.ShapeType;
        spell.SpellTargeting.ShapeWidth = dto.TargetingDto?.ShapeWidth ?? spell.SpellTargeting.ShapeWidth;

        spell.CastingRequirements.Verbal = dto.CastRequirementsDto?.Verbal ?? spell.CastingRequirements.Verbal;
        spell.CastingRequirements.Somatic = dto.CastRequirementsDto?.Somatic ?? spell.CastingRequirements.Somatic;
        spell.CastingRequirements.Materials = dto.CastRequirementsDto?.Materials ?? spell.CastingRequirements.Materials;
        spell.CastingRequirements.MaterialCost = dto.CastRequirementsDto?.MaterialCost ?? spell.CastingRequirements.MaterialCost;
        spell.CastingRequirements.MaterialsConsumed = dto.CastRequirementsDto?.MaterialsConsumed ?? spell.CastingRequirements.MaterialsConsumed;

        await repo.UpdateAsync(spell);
        logger.LogInformation("Successfully updated spell, Name: {SpellName} ID: {SpellId}", spell.Name, id);
        return spell;
    }

    // TODO move sorting logic to repository when implementing database level sorting
    public ICollection<Spell> SortBy(ICollection<Spell> spells, string sortFilter, bool descending = false)
    {
        if(!TryResolveValue<SortSpellOption>(sortFilter, out string? resolved))
            return spells;

        return resolved switch
        {
            SortSpellOption.Name => OrderByMany(spells, [(s => s.Name)], descending),
            SortSpellOption.Level => OrderByMany(spells, [(s => s.Level), (s => s.Name)], descending),
            SortSpellOption.CastingTime => OrderByMany(spells, [(s => s.CastingTime), (s => s.CastingTimeValue!), (s => s.Name)], descending),
            SortSpellOption.Duration => OrderByMany(spells, [(s => s.Duration), (s => s.DurationValue!), (s => s.Name)], descending),
            SortSpellOption.Target => OrderByMany(spells, [(s => s.SpellTargeting.TargetType), (s => s.Name)], descending),
            SortSpellOption.Range => OrderByMany(spells, [(s => s.SpellTargeting.Range), (s => s.SpellTargeting.RangeValue!), (s => s.Name)], descending),
            _ => throw new ValidationException($"Invalid sort option: {sortFilter}")
        };
    }

    // TODO move filtering logic to repository when implementing database level filtering
    public async Task<ICollection<Spell>> FilterAllAsync(SpellFilterDto dto)
    {
        if (dto.Name is not null)
            dto.Name = NormalizationUtil.NormalizeWhiteSpace(dto.Name);
        if (dto.MinLevel is not null && dto.MaxLevel is not null && dto.MinLevel > dto.MaxLevel)
            throw new ValidationException("Maximum level must be greater than or equal to minimum level");
        if (dto.MinLevel is not null && dto.MinLevel < 0)
            throw new ValidationException("Minimum level must be greater than or equal to zero");
        if (dto.MaxLevel is not null && dto.MaxLevel < 0)
            throw new ValidationException("Maximum level must be greater than or equal to zero");
        if (dto.ClassIds != null)
        {
            if (dto.ClassIds.HasDuplicates())
                throw new ValidationException($"Duplicate class ids found in ClassIds.");
    
            foreach (var id in dto.ClassIds)
            {
                if(!await classRepo.ExistsAsync(id))
                    throw new NotFoundException($"Class with id {id} does not exist.");
            }
        }

        var dtoSchools = dto.MagicSchools != null ? ResolveValueOrThrow<MagicSchool>(dto.MagicSchools) : null;
        var dtoTargetTypes = dto.TargetTypes != null ? ResolveValueOrThrow<TargetType>(dto.TargetTypes) : null;
        var dtoSpellRanges = dto.Range != null ? ResolveValueOrThrow<SpellRange>(dto.Range) : null;
        var dtoDurations = dto.Durations != null ? ResolveValueOrThrow<SpellDuration>(dto.Durations) : null;
        var dtoCastTimes = dto.CastingTimes != null ? ResolveValueOrThrow<CastingTime>(dto.CastingTimes) : null;
        var dtoSpellTypes = dto.SpellTypes != null ? ResolveValueOrThrow<SpellType>(dto.SpellTypes) : null;
        var dtoDamageTypes = dto.DamageTypes != null ? ResolveValueOrThrow<DamageType>(dto.DamageTypes) : null;

        var filter = new SpellFilter()
        {
            Name = dto.Name,
            MinLevel = dto.MinLevel,
            MaxLevel = dto.MaxLevel,
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
            throw new ValidationException($"Maximum level {filter.MaxLevel} must be greater than or equal to minimum level");
        if (filter.Name is not null)
            filter.Name = NormalizationUtil.NormalizeWhiteSpace(filter.Name);

        return await repo.FilterAllAsync(filter);
    }
}