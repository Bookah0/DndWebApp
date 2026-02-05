namespace DndWebApp.Api.Repositories.Implemented.Spells;

public class SpellFilter
{
    public required string? Name { get; set; }
    public required bool? IsHomebrew { get; set; }
    public required int? MinLevel { get; set; }
    public required int? MaxLevel { get; set; }
    public required ICollection<int>? ClassIds { get; set; }
    public required ICollection<string>? Durations { get; set; }
    public required ICollection<string>? CastingTimes { get; set; }
    public required ICollection<string>? MagicSchools { get; set; }
    public required ICollection<string>? SpellTypes { get; set; }
    public required ICollection<string>? TargetType { get; set; }
    public required ICollection<string>? Range { get; set; }
    public required ICollection<string>? DamageTypes { get; set; }
}