using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

/// <summary>
/// Contains primitive data from the base Feature class
/// </summary>
public class BaseFeaturePrimitiveDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
}

/// <summary>
/// Contains primitive data from the Trait, ClassFeature and BackgroundFeature class
/// </summary>
public class FeaturePrimitiveDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public required int FromEntityId { get; set; }
}

/// <summary>
/// Contains primitive data from the Feat class
/// </summary>
public class FeatPrimitiveDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public string Prerequisite { get; set; } = "";
}