using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

public class BackgroundDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public required ICollection<int> StartingItemIds { get; set; }
    public required ICollection<StartingItemOptionDto> StartingItemChoices { get; set; }
    public required CurrencyDto Currency { get; set; }
}

public class StartingItemOptionDto
{
    public required string Description { get; set; }
    public required ICollection<int> ItemOptionIds { get; set; }
}