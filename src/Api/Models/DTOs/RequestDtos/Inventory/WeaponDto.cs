using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.Inventory;

public class WeaponDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int Weight { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int Value { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string WeaponCategory { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string WeaponType { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string DamageDice { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int Range { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string MainDamageType { get; set; }

    public ICollection<string> OtherDamageTypes { get; set; } = [];

    public ICollection<string> Properties { get; set; } = [];

    [MinLength(1)]
    [MaxLength(50)]
    public string? VersitileDamageDice { get; set; }

    [Range(1, int.MaxValue)]
    public int? LongRange { get; set; }

    [MaxLength(50)]
    public string? Rarity { get; set; }

    public bool? RequiresAttunement { get; set; }

    public bool? IsHomebrew { get; set; }
}