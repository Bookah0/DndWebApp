using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

public class BackgroundFeatureDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public required int FromId { get; set; }
}

