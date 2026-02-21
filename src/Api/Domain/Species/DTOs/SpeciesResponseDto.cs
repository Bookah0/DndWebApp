namespace Api.Domain.Species.DTOs;

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