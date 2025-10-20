
using DndWebApp.Api.Models.Items.Enums;

namespace DndWebApp.Api.Models.Items;

// Based on:
// https://www.dnd5eapi.co/api/2014/equipment
// https://www.dnd5eapi.co/api/2014/magic-items/
// https://api.open5e.com/v1/magicitems/
// https://api.open5e.com/v1/weapons/
// https://api.open5e.com/v1/armor/
public class Item
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ItemCategory Catagories { get; set; }

    public ItemRarity? Rarity { get; set; }
    public bool RequiresAttunement { get; set; } = false;
    public int Weight { get; set; }
    public int Value { get; set; }
    public int Quantity { get; set; } = 1;
    public bool IsHomebrew { get; set; } = false;
}






