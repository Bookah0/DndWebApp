using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services.Spells;

public class SpellService : IService<Spell, SpellDto, SpellDto>
{
    protected SpellRepository repo;
    protected AppDbContext context;

    public SpellService(SpellRepository repo, AppDbContext context)
    {
        this.context = context;
        this.repo = repo;
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
        var dtoSpellTypes = ValidationUtil.ParseEnumsOrThrow<SpellType>(dto.Types);
        var dtoDamageTypes = ValidationUtil.ParseEnumsOrThrow<DamageType>(dto.DamageTypes);

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

    public async Task<ICollection<Spell>> FilterAllAsync(SpellFilter filter)
    {
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
        var dtoSpellTypes = ValidationUtil.ParseEnumsOrThrow<SpellType>(dto.Types);
        var dtoDamageTypes = ValidationUtil.ParseEnumsOrThrow<DamageType>(dto.DamageTypes);

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