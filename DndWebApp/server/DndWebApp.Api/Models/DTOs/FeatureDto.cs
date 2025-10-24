using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

/// <summary>
/// Contains primitive data from the Trait, ClassFeature and BackgroundFeature class
/// </summary>
public class FeatureDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public required int FromId { get; set; }
}

