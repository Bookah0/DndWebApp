namespace DndWebApp.Api.Models.DTOs;

public class AlignmentDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public required string Description { get; set; }
}