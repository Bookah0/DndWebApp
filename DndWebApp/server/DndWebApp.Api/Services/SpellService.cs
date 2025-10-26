using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Services.Utils;
namespace DndWebApp.Api.Services;

public class SpellService : IService<Spell, SpellDto, SpellDto>
{
    protected IRepository<Spell> repo;
    protected AppDbContext context;

    public SpellService(IRepository<Spell> repo, AppDbContext context)
    {
        this.context = context;
        this.repo = repo;
    }

    public async Task<Spell> CreateAsync(SpellDto dto)
    {
        ValidationUtil.ValidateRequired(dto.Name, "Name");
        ValidationUtil.ValidateRequired(dto.Description, "Description");
        ValidationUtil.ValidateRequired(dto.Duration, "Duration");
        ValidationUtil.ValidateRequired(dto.CastingTime, "CastingTime");
        ValidationUtil.ValidateRequired(dto.MagicSchool, "MagicSchool");

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
            SpellFactory.CreateSpellDamage(dto.DamageRoll, dtoDamageTypes),
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

    public async Task<Spell> GetByIdAsync(int id)
    {
        var spell = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Spell could not be found");
        return spell;
    }

    public async Task UpdateAsync(SpellDto dto)
    {
        var spell = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Spell could not be found");

        ValidationUtil.ValidateRequired(dto.Name, "Name");
        ValidationUtil.ValidateRequired(dto.Description, "Description");
        ValidationUtil.ValidateRequired(dto.Duration, "Duration");
        ValidationUtil.ValidateRequired(dto.CastingTime, "CastingTime");
        ValidationUtil.ValidateRequired(dto.MagicSchool, "MagicSchool");

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
        spell.SpellDamage = SpellFactory.CreateSpellDamage(dto.DamageRoll, dtoDamageTypes);
        spell.CastingRequirements = SpellFactory.CreateCastingRequirments(dto.Verbal, dto.Somatic, dto.Materials, dto.MaterialCost, dto.MaterialsConsumed);

        await repo.UpdateAsync(spell);
        await context.SaveChangesAsync();
    }
}