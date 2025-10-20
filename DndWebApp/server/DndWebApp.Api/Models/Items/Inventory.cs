using DndWebApp.Api.Models.Items.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Items;

public class Inventory
{
    public int Id { get; set; }

    // Categories: Gem, Jewelry, WondrousItem, TradeGood
    public List<Item> Treasure { get; set; } = [];

    // Categories: Ammunition, Armor, Ring, Rod, Shield, SpellcastingFocus, Staff, Wand, Weapon
    public List<Item> Equipment { get; set; } = [];

    // Categories: Poison, Potion, Scroll
    public List<Item> Consumables { get; set; } = [];

    // Categories: AdventuringGear, EquipmentPack, Tools
    public List<Item> Gear { get; set; } = [];

    // Categories: LandVehicle, Mount, WaterborneVehicle
    public List<Item> Misc { get; set; } = [];

    public Currency Currency { get; set; } = new();

    public int TotalWeight { get; set; }
    public int MaxWeight { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public EquippedItems EquippedItems { get; set; } = new();
}

[Owned]
public class EquippedItems
{
    // Equipped items
    public Weapon? EquippedMainHand { get; set; }
    public Item? EquippedOffHand { get; set; }
    public Item? EquippedRanged { get; set; }
    public Armor? EquippedArmor { get; set; }
    public Item? EquippedOnHead { get; set; }
    public Item? EquippedOnWaist { get; set; }
    public Item? EquippedOnHands { get; set; }
    public Item? EquippedOnFeet { get; set; }

    public List<Item> EquippedRings { get; set; } = [];
    public int RingCap { get; set; } = 2;

    public List<Item> EquippedNecklace { get; set; } = [];
    public int NecklaceCap { get; set; } = 1;

    public List<Item> EquippedOnBack { get; set; } = [];
    public int BackEquipmentCap { get; set; } = 1;

    // Magical
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