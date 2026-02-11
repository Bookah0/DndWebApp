using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.Spells;

public class CreateSpellRequestDto
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
    [Range(0, 9)]
    public required int Level { get; set; }

    [MaxLength(1000)]
    public string EffectsAtHigherLevels { get; set; } = "";

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public required string Duration { get; set; }

    [Range(0, int.MaxValue)]
    public int? DurationValue { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public required string CastingTime { get; set; }

    [Range(0, int.MaxValue)]
    public int CastingTimeValue { get; set; }

    [MaxLength(500)]
    public required string ReactionCondition { get; set; }

    [MaxLength(100)]
    public string MagicSchool { get; set; } = "";
    public List<string> SpellTypes { get; set; } = [];

    [MaxLength(50)]
    public string DamageRoll { get; set; } = "";
    public List<string> DamageTypes { get; set; } = [];

    [Required]
    public required CreateSpellTargetingDto TargetingDto { get; set; }
    public CreateCastingRequirementsDto CastRequirementsDto { get; set; } = new();
}

public class UpdateSpellRequestDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Range(0, 9)]
    public int? Level { get; set; }

    [MaxLength(1000)]
    public string? EffectsAtHigherLevels { get; set; }

    [MaxLength(200)]
    public string? Duration { get; set; }

    [MaxLength(200)]
    public string? CastingTime { get; set; }

    [MaxLength(500)]
    public string? ReactionCondition { get; set; }
    
    [MaxLength(100)]
    public string? MagicSchool { get; set; }

    [Range(0, int.MaxValue)]
    public int? DurationValue { get; set; }

    [Range(0, int.MaxValue)]
    public int? CastingTimeValue { get; set; }

    [MaxLength(50)]
    public string? DamageRoll { get; set; }
    public UpdateSpellTargetingDto? TargetingDto { get; set; }
    public UpdateCastingRequirementsDto? CastRequirementsDto { get; set; }
}

public class CreateSpellTargetingDto
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

    [MaxLength(50)]
    public string ShapeType { get; set; } = "";

    [MaxLength(50)]
    public string ShapeWidth { get; set; } = "";

    [MaxLength(50)]
    public string ShapeLength { get; set; } = "";
}

public class UpdateSpellTargetingDto
{
    [MaxLength(100)]
    public string? TargetType { get; set; }

    [MaxLength(100)]
    public string? Range { get; set; }

    [Range(0, int.MaxValue)]
    public int? RangeValue { get; set; }

    [MaxLength(50)]
    public string? ShapeType { get; set; }

    [MaxLength(50)]
    public string? ShapeWidth { get; set; }

    [MaxLength(50)]
    public string? ShapeLength { get; set; }
}

public class CreateCastingRequirementsDto
{
    public bool Verbal { get; set; } = false;
    public bool Somatic { get; set; } = false;

    [MaxLength(200)]
    public string Materials { get; set; } = "";

    [Range(0, int.MaxValue)]
    public int MaterialCost { get; set; } = 0;
    public bool MaterialsConsumed { get; set; } = false;
}

public class UpdateCastingRequirementsDto
{
    public bool? Verbal { get; set; }
    public bool? Somatic { get; set; }

    [MaxLength(200)]
    public string? Materials { get; set; }

    [Range(0, int.MaxValue)]
    public int? MaterialCost { get; set; }
    public bool? MaterialsConsumed { get; set; }
}