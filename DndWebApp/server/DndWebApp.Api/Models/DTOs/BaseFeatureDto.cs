namespace DndWebApp.Api.Models.DTOs;

/// <summary>
/// Contains primitive data from the base Feature class
/// </summary>
public class BaseFeatureDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
}