using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Items;

[Owned]
public class Inventory
{
    public StoredItems StoredItems { get; set; } = new();
    public Currency Currency { get; set; } = new();
    public EquippedItems EquippedItems { get; set; } = new();

    public int TotalWeight { get; set; }
    public int MaxWeight { get; set; }
}

[Owned]
public class StoredItems
{
    public ICollection<Item> Treasure { get; set; } = [];      // Categories: Gem, Jewelry, WondrousItem, TradeGood
    public ICollection<Item> Equipment { get; set; } = [];     // Categories: Ammunition, Armor, Ring, Rod, Shield, SpellcastingFocus, Staff, Wand, Weapon
    public ICollection<Item> Consumables { get; set; } = [];   // Categories: Poison, Potion, Scroll
    public ICollection<Item> Gear { get; set; } = [];          // Categories: AdventuringGear, EquipmentPack, Tools
    public ICollection<Item> Misc { get; set; } = [];          // Categories: LandVehicle, Mount, WaterborneVehicle, null ItemCategory
}

[Owned]
public class EquippedItems
{
    public Weapon? EquippedMainHand { get; set; }
    public Item? EquippedOffHand { get; set; }
    public Item? EquippedRanged { get; set; }
    public Armor? EquippedArmor { get; set; }
    public Item? EquippedOnHead { get; set; }
    public Item? EquippedOnWaist { get; set; }
    public Item? EquippedOnHands { get; set; }
    public Item? EquippedOnFeet { get; set; }

    public ICollection<Item> EquippedRings { get; set; } = [];
    public int RingCap { get; set; } = 2;

    public ICollection<Item> EquippedNecklace { get; set; } = [];
    public int NecklaceCap { get; set; } = 1;

    public ICollection<Item> EquippedOnBack { get; set; } = [];
    public int BackEquipmentCap { get; set; } = 1;

    public Item? EquippedArcaneFocus { get; set; }
    public Item? EquippedHolySymbol { get; set; }
    public int AttunedItems { get; set; } = 0;
}

[Owned]
public class Currency
{
    public int Brass { get; set; }
    public int Copper { get; set; }
    public int Silver { get; set; }
    public int Gold { get; set; }
    public int Platinum { get; set; }
    public int Electrum { get; set; }
}