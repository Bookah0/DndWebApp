using System.ComponentModel.DataAnnotations;
using Api.Validation.AllowedValues.Items;

namespace Api.Models.DTOs.RequestDtos.Inventory;

public class CreateItemRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Range(0, int.MaxValue)]
    public int? Value { get; set; }

    public required ICollection<string> Categories { get; set; } 

    [MinLength(1)]
    [MaxLength(50)]
    public required string Rarity { get; set; } = ItemRarity.Common;

    public bool RequiresAttunement { get; set; } = false;

    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;
}

public class UpdateItemRequestDto
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

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}