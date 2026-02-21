namespace Api.Domain.Alignments.DTOs;

public class AlignmentResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public required string Description { get; set; }
}