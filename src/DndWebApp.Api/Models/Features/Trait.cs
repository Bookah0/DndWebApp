
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Services.Util.Interfaces;

namespace DndWebApp.Api.Models.Features;

// Based on https://5e-bits.github.io/docs/api/
public class Trait : AFeature, IOwnedByEntity
{
    public required Species FromRace { get; set; }
    public required int RaceId { get; set; }
    public int OwnerId => RaceId;
}