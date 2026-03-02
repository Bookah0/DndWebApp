using Api.Domain.Shared.DTOs;

namespace Api.Domain.Species.DTOs;

public abstract class SpeciesResponseDto : CreatableEntityResponseDto
{
    public required string Name { get; set; }
    public required SpeciesInfoDto Info { get; set; }
    public ICollection<int> TraitIds { get; set; } = [];
}

public class RaceResponseDto : SpeciesResponseDto
{
    public ICollection<int> SubraceIds { get; set; } = [];
}

public class SubraceResponseDto : SpeciesResponseDto
{
    public int ParentRaceId { get; set; }
}