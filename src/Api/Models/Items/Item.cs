using Api.Models.Items.Constants;

namespace Api.Models.Items;

// Based on:
// https://www.dnd5eapi.co/api/2014/equipment
// https://www.dnd5eapi.co/api/2014/magic-items/
// https://api.open5e.com/v1/magicitems/
// https://api.open5e.com/v1/weapons/
// https://api.open5e.com/v1/armor/
public class Item : CreatableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ICollection<string> Categories { get; set; }
    public string Rarity { get; set; } = ItemRarity.Common;
    public bool RequiresAttunement { get; set; } = false;
    public int Weight { get; set; }
    public int Value { get; set; }
    public int Quantity { get; set; } = 1;
}






