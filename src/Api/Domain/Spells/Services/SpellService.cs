using Api.Domain.Classes.Repositories;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Enums.Damage;
using Api.Domain.Shared.Enums.Spells;
using Api.Domain.Shared.Utils;
using Api.Domain.Spells.DTOs;
using Api.Domain.Spells.Models;
using Api.Domain.Spells.Repositories;
using Api.Domain.Users.Services;
using Api.Infrastructure.Middleware.ExceptionHandling;
using static Api.Infrastructure.Validation.ValuesValidator;

namespace Api.Domain.Spells.Services;

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

        var dtoTargetType = NormalizeValue<TargetType>(dto.TargetingDto.TargetType);
        var dtoSpellRange = NormalizeValue<SpellRange>(dto.TargetingDto.Range);
        var dtoDuration = NormalizeValue<SpellDuration>(dto.Duration);
        var dtoCastTime = NormalizeValue<CastingTime>(dto.CastingTime);

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
            MagicSchool = NormalizeValue<MagicSchool>(dto.MagicSchool, throwOnError: false),
            SpellTypes = NormalizeValue<SpellType>(dto.SpellTypes, throwOnError: false),
            DamageRoll = dto.DamageRoll,
            DamageTypes = NormalizeValue<DamageType>(dto.DamageTypes, throwOnError: false),
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

    public async Task<(int, ICollection<Spell>)> GetFilteredAsync(SpellFilterDto filter, PaginationRequestDto pagination) 
    {
        await ValidatieFilterAsync(filter);
        var (count, filtered) = await repo.GetFilteredAsync(filter, pagination);

        if(!filtered.HasContent() && count > 0)
            throw new ValidationException("Page does not contain any elements");

        return (count, filtered);
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
            spell.MagicSchool = NormalizeValue<MagicSchool>(dto.MagicSchool, throwOnError: false);

        if(dto.TargetingDto?.TargetType is not null)
            spell.SpellTargeting.TargetType = NormalizeValue<TargetType>(dto.TargetingDto.TargetType, throwOnError: false);

        if(dto.TargetingDto?.Range is not null)
            spell.SpellTargeting.Range = NormalizeValue<SpellRange>(dto.TargetingDto.Range, throwOnError: false);

        if(dto.Duration is not null)
            spell.Duration = NormalizeValue<SpellDuration>(dto.Duration, throwOnError: false);

        if(dto.CastingTime is not null)
            spell.CastingTime = NormalizeValue<CastingTime>(dto.CastingTime, throwOnError: false);

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

    public async Task ValidatieFilterAsync(SpellFilterDto dto)
    {
        if (dto.MinLevel is not null && dto.MaxLevel is not null && dto.MinLevel > dto.MaxLevel)
            throw new ValidationException("Maximum level must be greater than or equal to minimum level");
        if (dto.MinLevel is not null && dto.MinLevel < 0)
            throw new ValidationException("Minimum level must be greater than or equal to zero");
        if (dto.MaxLevel is not null && dto.MaxLevel < 0)
            throw new ValidationException("Maximum level must be greater than or equal to zero");
        if (dto.ClassId != null)
        {
            foreach (var id in dto.ClassId)
            {
                if(!await classRepo.ExistsAsync(id))
                    throw new NotFoundException($"Class with id {id} does not exist.");
            }
        }

        dto.MagicSchool = NormalizeValue<MagicSchool>(dto.MagicSchool);
        dto.TargetType = NormalizeValue<TargetType>(dto.TargetType);
        dto.Range = NormalizeValue<SpellRange>(dto.Range);
        dto.Duration = NormalizeValue<SpellDuration>(dto.Duration);
        dto.CastingTime = NormalizeValue<CastingTime>(dto.CastingTime);
        dto.SpellType = NormalizeValue<SpellType>(dto.SpellType);
        dto.DamageType = NormalizeValue<DamageType>(dto.DamageType);
    }
}