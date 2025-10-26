using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Spells;

// Based on https://api.open5e.com/v1/spells
public class Spell
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public required int Level { get; set; }
    public string EffectsAtHigherLevels { get; set; } = "";
    public ICollection<Class> Classes { get; set; } = [];
    public required SpellDuration Duration { get; set; }
    public required CastingTime CastingTime { get; set; }
    public string ReactionCondition { get; set; } = "";
    public required MagicSchool MagicSchool { get; set; }

    public SpellType SpellTypes { get; set; } = SpellType.Normal;
    public required SpellTargeting SpellTargeting { get; set; }  
    public SpellDamage SpellDamage { get; set; } = new();
    public CastingRequirements CastingRequirements { get; set; } = new();  
}

[Owned]
public class SpellTargeting
{
    public required SpellTargetType TargetType { get; set; }
    public required SpellRange Range { get; set; }
    public int RangeValue { get; set; } = 0;
    public string? ShapeType { get; set; }
    public string? ShapeWidth { get; set; }
    public string? ShapeLength { get; set; }
}

[Owned]
public class SpellDamage
{
    public string DamageRoll { get; set; } = "";
    public DamageType DamageTypes { get; set; }
}

[Owned]
public class CastingRequirements
{
    public bool Verbal { get; set; }
    public bool Somatic { get; set; }
    public string? Materials { get; set; }
    public int? MaterialCost { get; set; }
    public bool MaterialsConsumed { get; set; }
}