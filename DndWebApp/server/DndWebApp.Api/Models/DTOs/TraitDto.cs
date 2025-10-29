namespace DndWebApp.Api.Models.DTOs;

public class TraitDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public required int RaceId { get; set; }
}