namespace Api.Models.DTOs.ResponseDtos;

public class SubraceResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public ICollection<int> TraitIds { get; set; } = [];
    public int ParentRaceId { get; set; }
}