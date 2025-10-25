
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Models.Characters;

// Based on https://5e-bits.github.io/docs/api/
public class Trait : AFeature
{
    public required Species FromRace { get; set; }
    public required int RaceId { get; set; }
}