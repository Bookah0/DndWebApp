using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;

namespace DndWebApp.Api.Repositories.Spells;

public class SpellFilter
{
    public required string? Name { get; set; }
    public required bool? IsHomebrew { get; set; }
    public required int? MinLevel { get; set; }
    public required int? MaxLevel { get; set; }
    public required ICollection<int>? ClassIds { get; set; }
    public required ICollection<SpellDuration>? Durations { get; set; }
    public required ICollection<CastingTime>? CastingTimes { get; set; }
    public required ICollection<MagicSchool>? MagicSchools { get; set; }
    public required ICollection<SpellType>? SpellTypes { get; set; }
    public required ICollection<SpellTargetType>? TargetType { get; set; }
    public required ICollection<SpellRange>? Range { get; set; }
    public required ICollection<DamageType>? DamageTypes { get; set; }

    /*
    Not sure if these should be added:
    public int? MinRangeValue { get; set; }
    public int? MaxRangeValue { get; set; }
    public bool? IsAoe { get; set; }
    public bool? Verbal { get; set; }
    public bool? Somatic { get; set; }
    public bool? Materials { get; set; }
    */
}