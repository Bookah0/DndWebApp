using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.Features;

// Based on https://www.dnd5eapi.co/api/2014/backgrounds/{backgroundId}/benefits/
public class BackgroundFeature : AFeature
{
    public Background? Background { get; set; }
    public required int BackgroundId { get; set; }
}
