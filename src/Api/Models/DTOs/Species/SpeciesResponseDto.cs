using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Models.DTOs.ResponseDtos;

public class RaceResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required SpeciesInfoDto Info { get; set; }
    public ICollection<int> TraitIds { get; set; } = [];
    public ICollection<int> SubraceIds { get; set; } = [];
}

public class SubraceResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required SpeciesInfoDto Info { get; set; }
    public ICollection<int> TraitIds { get; set; } = [];
    public int ParentRaceId { get; set; }
}