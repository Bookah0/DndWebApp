namespace DndWebApp.Api.Models.DTOs;

public class SpellDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public required int Level { get; set; }
    public required string EffectsAtHigherLevels { get; set; }
    public required string Duration { get; set; }
    public required string CastingTime { get; set; }
    public required string ReactionCondition { get; set; }
    public required List<int> ClassIds{ get; set; } = [];
    public required string TargetType { get; set; }
    public required string MagicSchool { get; set; }
    public required string Range { get; set; }
    public required string Types { get; set; }
    public required string ShapeType { get; set; }
    public required string ShapeWidth { get; set; }
    public required string ShapeLength { get; set; }
    public required string DamageRoll { get; set; }
    public required string DamageTypes { get; set; }
    public required bool Verbal { get; set; }
    public required bool Somatic { get; set; }
    public required string Materials { get; set; }
    public int MaterialCost { get; set; }
    public required bool MaterialsConsumed { get; set; }
    public required int RangeValue { get; set; }
}