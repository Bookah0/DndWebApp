using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;


/// <summary>
/// Contains primitive data from the Background class
/// </summary>
public class BackgroundPrimitiveDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
}