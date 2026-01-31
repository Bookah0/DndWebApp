using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.Inventory;

public class ToolDto
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
    public required int Value { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string ToolCategory { get; set; }

    [MaxLength(50)]
    public string? Rarity { get; set; }

    public bool? RequiresAttunement { get; set; }

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }

    public bool? IsHomebrew { get; set; }
}