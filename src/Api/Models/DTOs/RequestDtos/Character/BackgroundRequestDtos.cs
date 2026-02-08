using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.RequestDtos.Inventory;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateBackgroundRequestDto
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
    public required CurrencyDto StartingCurrency { get; set; }
    public ICollection<int> FeatureIds { get; set; } = [];
    public ICollection<int> StartingItemIds { get; set; } = [];
    public ICollection<StartingItemOptionDto> StartingItemChoices { get; set; } = [];
}

public class UpdateBackgroundRequestDto
{  
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public string? Description { get; set; }
    public ICollection<int> FeatureIds { get; set; } = [];
    public ICollection<int> StartingItemIds { get; set; } = [];
    public ICollection<StartingItemOptionDto> StartingItemChoices { get; set; } = [];
    public CurrencyDto? StartingCurrency { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class StartingItemOptionDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<int> ItemOptionIds { get; set; }
}