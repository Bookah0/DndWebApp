using Api.Models.Characters;
using Api.Models.Spells.Constants;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.Spells;

// Based on https://api.open5e.com/v1/spells
public class Spell : CreatableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Level { get; set; }
    public string EffectsAtHigherLevels { get; set; } = "";
    public ICollection<BaseClass> Classes { get; set; } = [];
    public required string Duration { get; set; }
    public int DurationValue { get; set; }
    public required string CastingTime { get; set; }
    public int CastingTimeValue { get; set; }
    public string ReactionCondition { get; set; } = "";
    public string MagicSchool { get; set; } = "";
    public string DamageRoll { get; set; } = "";
    public ICollection<string> DamageTypes { get; set; } = [];
    public ICollection<string> SpellTypes { get; set; } = [];
    
    public required SpellTargeting SpellTargeting { get; set; }  
    public CastingRequirements CastingRequirements { get; set; } = new();  
}

[Owned]
public class SpellTargeting
{
    public required string TargetType { get; set; }
    public required string Range { get; set; }
    public required int RangeValue { get; set; }
    public string? ShapeType { get; set; }
    public string? ShapeWidth { get; set; }
    public string? ShapeLength { get; set; }
}

[Owned]
public class CastingRequirements
{
    public bool Verbal { get; set; } = false;
    public bool Somatic { get; set; } = false;
    public string Materials { get; set; } = "";
    public int MaterialCost { get; set; } = 0;
    public bool MaterialsConsumed { get; set; } = false;
}