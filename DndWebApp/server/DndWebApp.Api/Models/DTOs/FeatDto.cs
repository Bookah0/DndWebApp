using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Models.DTOs;

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