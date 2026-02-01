using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Services.Util.Interfaces;

namespace DndWebApp.Api.Models.Features;

// Based on https://www.dnd5eapi.co/api/2014/backgrounds/{backgroundId}/benefits/
public class BackgroundFeature : AFeature, IOwnedByEntity
{
    public Background? Background { get; set; }
    public required int BackgroundId { get; set; }
    public int OwnerId => BackgroundId;
}
