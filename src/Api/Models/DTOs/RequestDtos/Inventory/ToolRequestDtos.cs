using System.ComponentModel.DataAnnotations;
using Api.Models.Items.Constants;

namespace Api.Models.DTOs.RequestDtos.Inventory;

public class CreateToolRequestDto
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
    [MaxLength(100)]
    public required string ToolCategory { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string Rarity { get; set; } = ItemRarity.Common;

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }
    public bool RequiresAttunement { get; set; } = false;
}

public class UpdateToolRequestDto
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
    [MaxLength(100)]
    public string? ToolCategory { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? Rarity { get; set; }

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }
    public bool? RequiresAttunement { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class ToolPropertyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Title { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public required string Description { get; set; }

}

public class ToolActivityDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Title { get; set; }

    [Range(1, int.MaxValue)]
    public int? SkillId { get; set; }

    [Range(1, int.MaxValue)]
    public int? AbilityId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string DC { get; set; }
}