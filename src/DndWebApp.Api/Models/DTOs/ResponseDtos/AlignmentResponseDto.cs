namespace DndWebApp.Api.Models.DTOs.ResponseDtos;

public class AlignmentResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public required string Description { get; set; }
}