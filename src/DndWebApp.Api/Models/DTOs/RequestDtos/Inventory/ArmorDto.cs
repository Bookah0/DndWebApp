using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.Inventory;

public class ArmorDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Range(0, int.MaxValue)]
    public int Weight { get; set; }

    [Range(0, int.MaxValue)]
    public int Value { get; set; }

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

    public bool? IsHomebrew { get; set; }

    [Range(1, int.MaxValue)]
    public int? ModCap { get; set; }

    [Range(1, 30)]
    public int? StrengthScoreRequired { get; set; }

    public bool? StealthDisadvantage { get; set; }
}