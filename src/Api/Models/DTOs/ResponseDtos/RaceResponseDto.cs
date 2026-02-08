namespace Api.Models.DTOs.ResponseDtos;

public class RaceResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<int> SubraceIds { get; set; } = [];
    public ICollection<int> TraitIds { get; set; } = [];
}