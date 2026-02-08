using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Inventory;

public class CreateArmorRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }

    [Range(0, int.MaxValue)]
    public int? Value { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string Category { get; set; }

    [Required]
    [Range(1, 30)]
    public required int BaseArmorClass { get; set; }

    [Required]
    public required bool PlusDexMod { get; set; }

    [MaxLength(50)]
    public string? Rarity { get; set; }

    public bool? RequiresAttunement { get; set; }

    [Range(1, int.MaxValue)]
    public int? ModCap { get; set; }

    [Range(1, 30)]
    public int? StrengthScoreRequired { get; set; }

    public bool? StealthDisadvantage { get; set; }
}

public class UpdateArmorRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }

    [Range(0, int.MaxValue)]
    public int? Value { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? Category { get; set; }

    [Range(1, 30)]
    public int? BaseArmorClass { get; set; }

    public bool? PlusDexMod { get; set; }

    [MaxLength(50)]
    public string? Rarity { get; set; }

    public bool? RequiresAttunement { get; set; }

    [Range(1, int.MaxValue)]
    public int? ModCap { get; set; }

    [Range(1, 30)]
    public int? StrengthScoreRequired { get; set; }
    public bool? StealthDisadvantage { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}