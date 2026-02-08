
using Api.Models.Characters;
using Api.Services.Util.Interfaces;

namespace Api.Models.Features;

// Based on https://5e-bits.github.io/docs/api/
public class Trait : Feature
{
    public required Species FromRace { get; set; }
    public required int RaceId { get; set; }
}