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

    [MinLength(1)]
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

    [MinLength(1)]
    [MaxLength(500)]
    public required string ReactionCondition { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string MagicSchool { get; set; } = "";
    public List<string> SpellTypes { get; set; } = [];

    [MinLength(1)]
    [MaxLength(50)]
    public string DamageRoll { get; set; } = "";
    public List<string> DamageTypes { get; set; } = [];

    [Required]
    public required CreateSpellTargetingDto TargetingDto { get; set; }
    public CreateCastingRequirementsDto CastRequirementsDto { get; set; } = new();
}

public class UpdateSpellRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public string? Description { get; set; }

    [Range(0, 9)]
    public int? Level { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? EffectsAtHigherLevels { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string? Duration { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string? CastingTime { get; set; }

    [MinLength(1)]
    [MaxLength(500)]
    public string? ReactionCondition { get; set; }
    
    [MinLength(1)]
    [MaxLength(100)]
    public string? MagicSchool { get; set; }

    [Range(0, int.MaxValue)]
    public int? DurationValue { get; set; }

    [Range(0, int.MaxValue)]
    public int? CastingTimeValue { get; set; }

    [MinLength(1)]
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

    [MinLength(1)]
    [MaxLength(50)]
    public string ShapeType { get; set; } = "";

    [MinLength(1)]
    [MaxLength(50)]
    public string ShapeWidth { get; set; } = "";

    [MinLength(1)]
    [MaxLength(50)]
    public string ShapeLength { get; set; } = "";
}

public class UpdateSpellTargetingDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? TargetType { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string? Range { get; set; }

    [Range(0, int.MaxValue)]
    public int? RangeValue { get; set; }

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

public class CreateCastingRequirementsDto
{
    public bool Verbal { get; set; } = false;
    public bool Somatic { get; set; } = false;
    public string Materials { get; set; } = "";
    public int MaterialCost { get; set; } = 0;
    public bool MaterialsConsumed { get; set; } = false;
}

public class UpdateCastingRequirementsDto
{
    public bool? Verbal { get; set; }
    public bool? Somatic { get; set; }
    public string? Materials { get; set; }
    public int? MaterialCost { get; set; }
    public bool? MaterialsConsumed { get; set; }
}