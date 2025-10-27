using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.Spells.Enums;

namespace DndWebApp.Api.Repositories.Spells;

public class SpellFilter
{
    public string? Name { get; set; }
    public bool? IsHomebrew { get; set; }
    public int? MinLevel { get; set; }
    public int? MaxLevel { get; set; }
    public ICollection<int>? ClassIds { get; set; }
    public ICollection<SpellDuration>? Durations { get; set; }
    public ICollection<CastingTime>? CastingTimes { get; set; }
    public ICollection<MagicSchool>? MagicSchools { get; set; }
    public ICollection<SpellType>? SpellTypes { get; set; }
    public ICollection<SpellTargetType>? TargetType { get; set; }
    public ICollection<SpellRange>? Range { get; set; }
    public ICollection<DamageType>? DamageTypes { get; set; }

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