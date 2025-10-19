using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models.Characters;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class Feature : BenefitProvider
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public required Class FromClass { get; set; }
    public required string LevelWhenGained { get; set; }

}
