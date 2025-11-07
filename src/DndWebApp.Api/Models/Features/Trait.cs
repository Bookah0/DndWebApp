
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.Features;

// Based on https://5e-bits.github.io/docs/api/
public class Trait : AFeature
{
    public required Species FromRace { get; set; }
    public required int RaceId { get; set; }
}