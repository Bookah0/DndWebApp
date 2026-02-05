using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.Inventory;

public class ItemDto
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
    public required int Value { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string MainCategory { get; set; }

    public List<string> OtherCategories { get; set; } = [];

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string Rarity { get; set; }

    public bool? RequiresAttunement { get; set; }

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }

    public bool? IsHomebrew { get; set; }
}