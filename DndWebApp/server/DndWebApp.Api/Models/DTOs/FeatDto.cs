using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

/// <summary>
/// Contains primitive data from the Feat class
/// </summary>
public class FeatDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public string Prerequisite { get; set; } = "";
    public required int FromId { get; set; }
    public required FeatFromType FromType { get; set; }
}