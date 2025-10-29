using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.DTOs;

public class ClassFeatureDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public required int ClassLevelId { get; set; }
}

