using System.ComponentModel.DataAnnotations;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells.Enums;

namespace DndWebApp.Api.Models.DTOs.Spells;


public class SpellFilterDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    public bool? IsHomebrew { get; set; }

    [Range(0, 9)]
    public int? MinLevel { get; set; }

    [Range(0, 9)]
    public int? MaxLevel { get; set; }

    public ICollection<int>? ClassIds { get; set; }
    public ICollection<string>? Durations { get; set; }
    public ICollection<string>? CastingTimes { get; set; }
    public ICollection<string>? MagicSchools { get; set; }
    public ICollection<string>? SpellTypes { get; set; }
    public ICollection<string>? TargetTypes { get; set; }
    public ICollection<string>? Range { get; set; }
    public ICollection<string>? DamageTypes { get; set; }
}