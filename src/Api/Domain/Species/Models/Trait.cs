using Api.Domain.Shared.Models;

namespace Api.Domain.Species.Models;

// Based on https://5e-bits.github.io/docs/api/
public class Trait : Feature
{
    public required Species FromRace { get; set; }
    public required int RaceId { get; set; }
}