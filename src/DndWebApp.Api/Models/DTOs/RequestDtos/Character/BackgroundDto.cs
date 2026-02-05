using System.ComponentModel.DataAnnotations;
using DndWebApp.Api.Models.DTOs.Inventory;

namespace DndWebApp.Api.Models.DTOs.RequestDtos.Character;

public class BackgroundDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    public bool IsHomebrew { get; set; } = false;

    [Required]
    public required ICollection<int> StartingItemIds { get; set; }

    [Required]
    public required ICollection<StartingItemOptionDto> StartingItemChoices { get; set; }

    [Required]
    public required CurrencyDto Currency { get; set; }
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