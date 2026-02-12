using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Inventory;

public class CreateWeaponRequestDto
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
    [Range(0, int.MaxValue)]
    public int? Value { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string Rarity { get; set; }

    public bool RequiresAttunement { get; set; } = false;

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }

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
    [MaxLength(100)]
    public required string Slot { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public required string DamageDice { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int Range { get; set; }

    public ICollection<string> DamageTypes { get; set; } = [];

    public ICollection<string> Properties { get; set; } = [];

    [MaxLength(50)]
    public string VersitileDamageDice { get; set; } = "";

    [Range(1, int.MaxValue)]
    public int? LongRange { get; set; }
}

public class UpdateWeaponRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int? Value { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? Rarity { get; set; }

    public bool? RequiresAttunement { get; set; }

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string? WeaponCategory { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string? WeaponType { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string? Slot { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? DamageDice { get; set; }

    [Range(1, int.MaxValue)]
    public int? Range { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? VersitileDamageDice { get; set; }

    [Range(1, int.MaxValue)]
    public int? LongRange { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}