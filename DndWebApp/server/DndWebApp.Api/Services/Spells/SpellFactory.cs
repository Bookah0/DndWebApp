using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;

namespace DndWebApp.Api.Services.Spells;

public static class SpellFactory
{
    public static Spell Create(
        string name,
        string description,
        bool isHomebrew,
        int level,
        string effectsAtHigherLevels,
        string reactionCondition,
        SpellTargeting targeting,
        string DamageRoll,
        ICollection<DamageType> DamageTypes,
        CastingRequirements castingRequirements,
        SpellDuration duration,
        CastingTime castingTime,
        MagicSchool magicSchool,
        ICollection<SpellType> spellTypes
    )
    {
        return new()
        {
            Name = name,
            Description = description,
            IsHomebrew = isHomebrew,
            Level = level,
            EffectsAtHigherLevels = effectsAtHigherLevels,
            Duration = duration,
            CastingTime = castingTime,
            ReactionCondition = reactionCondition,
            SpellTargeting = targeting,
            MagicSchool = magicSchool,
            SpellTypes = spellTypes,
            DamageRoll = DamageRoll,
            DamageTypes = DamageTypes,
            CastingRequirements = castingRequirements
        };
    }

    public static CastingRequirements CreateCastingRequirments(bool Verbal, bool Somatic, string Materials, int MaterialCost, bool MaterialsConsumed)
    {
        return new()
        {
            Verbal = Verbal,
            Somatic = Somatic,
            Materials = Materials,
            MaterialCost = MaterialCost,
            MaterialsConsumed = MaterialsConsumed
        };
    }

    public static SpellTargeting CreateTargeting(SpellTargetType TargetType, SpellRange Range, int RangeValue, string ShapeType, string ShapeWidth, string ShapeLength)
    {
        return new()
        {
            TargetType = TargetType,
            Range = Range,
            RangeValue = RangeValue,
            ShapeType = ShapeType,
            ShapeWidth = ShapeWidth,
            ShapeLength = ShapeLength
        };
    }
}