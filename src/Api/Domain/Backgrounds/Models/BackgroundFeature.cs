using Api.Domain.Shared.Models;

namespace Api.Domain.Backgrounds.Models;

// Based on https://www.dnd5eapi.co/api/2014/backgrounds/{backgroundId}/benefits/
public class BackgroundFeature : Feature
{
    public Background? Background { get; set; }
    public required int BackgroundId { get; set; }
}
