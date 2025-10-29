using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Models.Characters;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class ClassFeature : AFeature
{
    public ClassLevel? ClassLevel { get; set; }
    public required int ClassLevelId { get; set; }
}
