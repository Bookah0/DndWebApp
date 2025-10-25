using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

public class BackgroundDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
}