using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Models.Characters;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class BackgroundFeature : Feature
{
    public required Background Background { get; set; }
    public required int BackgroundId { get; set; }
}
