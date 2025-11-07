namespace DndWebApp.Api.Models.DTOs;

public class SpellDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public required List<int> ClassIds { get; set; } = [];

    public required int Level { get; set; }
    public required string EffectsAtHigherLevels { get; set; }
    public required string Duration { get; set; }
    public required string CastingTime { get; set; }
    public required string ReactionCondition { get; set; }
    public required string MagicSchool { get; set; }
    public required List<string> Types { get; set; }
    public int DurationValue { get; set; }
    public int CastingTimeValue { get; set; }
    public required string DamageRoll { get; set; }
    public required List<string> DamageTypes { get; set; }
    public required SpellTargetingDto TargetingDto { get; set; }
    public CastingRequirementsDto CastRequirementsDto { get; set; } = new();
}

public class SpellTargetingDto
{
    public required string TargetType { get; set; }
    public required string Range { get; set; }
    public int RangeValue { get; set; } = 0;
    public string? ShapeType { get; set; }
    public string? ShapeWidth { get; set; }
    public string? ShapeLength { get; set; }
}

public class CastingRequirementsDto
{
    public bool Verbal { get; set; }
    public bool Somatic { get; set; }
    public string? Materials { get; set; }
    public int? MaterialCost { get; set; }
    public bool MaterialsConsumed { get; set; }
}