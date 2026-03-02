using Api.Domain.Shared.DTOs;

namespace Api.Domain.Spells.DTOs;

public class SpellResponseDto : CreatableEntityResponseDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Level { get; set; }
    public required string EffectsAtHigherLevels { get; set; }
    public required ICollection<int> ClassIds { get; set; }
    public required string Duration { get; set; }
    public required int? DurationValue { get; set; }
    public required string CastingTime { get; set; }
    public required int? CastingTimeValue { get; set; }
    public required string ReactionCondition { get; set; }
    public required string MagicSchool { get; set; }
    public required string DamageRoll { get; set; }
    public required ICollection<string> DamageTypes { get; set; }
    public required ICollection<string> SpellTypes { get; set; }
    
    public required SpellTargetingDto SpellTargeting { get; set; }  
    public required CastingRequirementsDto CastingRequirements { get; set; }
}

public class SpellTargetingDto
{
    public required string TargetType { get; set; }
    public required string Range { get; set; }
    public required int? RangeValue { get; set; }
    public required string ShapeType { get; set; }
    public required string ShapeWidth { get; set; }
    public required string ShapeLength { get; set; }
}

public class CastingRequirementsDto
{
    public required bool Verbal { get; set; }
    public required bool Somatic { get; set; }
    public required string Materials { get; set; }
    public required int MaterialCost { get; set; }
    public required bool MaterialsConsumed { get; set; }
}