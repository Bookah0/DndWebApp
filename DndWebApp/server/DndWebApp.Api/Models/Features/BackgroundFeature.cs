using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.Features;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class BackgroundFeature : AFeature
{
    public required Background Background { get; set; }
    public required int BackgroundId { get; set; }
}
