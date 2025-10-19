using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells.Enums;

namespace DndWebApp.Api.Models.Spells;

// Based on https://api.open5e.com/v1/spells
public class Spell
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Level { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public string EffectsAtHigherLevels { get; set; } = "";

    public string? RangeString { get; set; }
    public double? RangeValue { get; set; }
    public string? RangeUnit { get; set; }
    public string? Duration { get; set; }
    public bool IsRitual { get; set; }
    public bool NeedsConcentration { get; set; }
    public required string CastingTime { get; set; }
    public string ReactionCondition { get; set; } = "";
    public required string TargetType { get; set; }
    public required bool IsAttackRoll { get; set; }
    public string DamageRoll { get; set; } = "";
    public List<DamageType> DamageTypes { get; set; } = [];

    public bool Verbal { get; set; }
    public bool Somatic { get; set; }
    public string? Materials { get; set; }
    public int? MaterialCost { get; set; }
    public bool MaterialsConsumed { get; set; }

    public required MagicSchool MagicSchool { get; set; }
    public List<Class> Classes { get; set; } = [];
}
