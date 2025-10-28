using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells.Enums;

namespace DndWebApp.Api.Models.DTOs;

public class SpellFilterDto
{
    public string? Name { get; set; }
    public bool? IsHomebrew { get; set; }
    public int? MinLevel { get; set; }
    public int? MaxLevel { get; set; }
    public ICollection<int>? ClassIds { get; set; }
    public ICollection<string>? Durations { get; set; }
    public ICollection<string>? CastingTimes { get; set; }
    public ICollection<string>? MagicSchools { get; set; }
    public ICollection<string>? SpellTypes { get; set; }
    public ICollection<string>? TargetTypes { get; set; }
    public ICollection<string>? Range { get; set; }
    public ICollection<string>? DamageTypes { get; set; }

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