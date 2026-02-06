using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.Spells;

public class SpellDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public required string Description { get; set; }

    [Required]
    public required bool IsHomebrew { get; set; }

    [Required]
    public required List<int> ClassIds { get; set; } = [];

    [Required]
    [Range(0, 9)]
    public required int Level { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string EffectsAtHigherLevels { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public required string Duration { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public required string CastingTime { get; set; }

    [MinLength(1)]
    [MaxLength(500)]
    public required string ReactionCondition { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string MagicSchool { get; set; }

    [Required]
    public required List<string> Types { get; set; }

    [Range(0, int.MaxValue)]
    public int DurationValue { get; set; }

    [Range(0, int.MaxValue)]
    public int CastingTimeValue { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string DamageRoll { get; set; }

    [Required]
    public required List<string> DamageTypes { get; set; }

    [Required]
    public required SpellTargetingDto TargetingDto { get; set; }

    public CastingRequirementsDto CastRequirementsDto { get; set; } = new();
}

public class SpellTargetingDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string TargetType { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Range { get; set; }

    [Range(0, int.MaxValue)]
    public int RangeValue { get; set; } = 0;

    [MinLength(1)]
    [MaxLength(50)]
    public string? ShapeType { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? ShapeWidth { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? ShapeLength { get; set; }
}

public class CastingRequirementsDto
{
    public bool Verbal { get; set; }

    public bool Somatic { get; set; }

    [MinLength(1)]
    [MaxLength(500)]
    public string? Materials { get; set; }

    [Range(0, int.MaxValue)]
    public int? MaterialCost { get; set; }

    public bool MaterialsConsumed { get; set; }
}