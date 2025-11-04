using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.Features;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class ClassFeature : AFeature
{
    public ClassLevel? ClassLevel { get; set; }
    public required int ClassLevelId { get; set; }
}
