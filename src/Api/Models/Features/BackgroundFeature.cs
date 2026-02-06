using Api.Models.Characters;
using Api.Services.Util.Interfaces;

namespace Api.Models.Features;

// Based on https://www.dnd5eapi.co/api/2014/backgrounds/{backgroundId}/benefits/
public class BackgroundFeature : Feature
{
    public Background? Background { get; set; }
    public required int BackgroundId { get; set; }
}
