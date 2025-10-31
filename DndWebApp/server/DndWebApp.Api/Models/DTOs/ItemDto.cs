using DndWebApp.Api.Models.Items.Enums;

namespace DndWebApp.Api.Models.DTOs;

public class ItemDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Value { get; set; }
    public required string MainCategory { get; set; }
    public List<string>? OtherCategories { get; set; }
    public string? Rarity { get; set; }
    public bool? RequiresAttunement { get; set; }
    public int? Weight { get; set; }
    public bool? IsHomebrew { get; set; }
}