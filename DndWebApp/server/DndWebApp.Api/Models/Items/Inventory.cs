using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Items;

public class Inventory
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public required Currency Currency { get; set; }
    public int TotalWeight { get; set; }
    public int MaxWeight { get; set; }
    public int AttunedItems { get; set; } = 0;
        
    // --- Stored Items ---
    public ICollection<Item> Treasures { get; set; } = [];        // Gem, Jewelry, WondrousItem, TradeGood
    public ICollection<Item> Equipment { get; set; } = [];        // Ammunition, Armor, Ring, Rod, Shield, SpellcastingFocus, Staff, Wand, Weapon
    public ICollection<Item> Consumables { get; set; } = [];      // Poison, Potion, Scroll
    public ICollection<Tool> Gear { get; set; } = [];             // AdventuringGear, EquipmentPack, Tools
    public ICollection<Item> Misc { get; set; } = [];             // LandVehicle, Mount, WaterborneVehicle, uncategorized items

    // --- Equipped Items ---
    public Weapon? EquippedMainHand { get; set; }
    public Weapon? EquippedOffHand { get; set; }
    public Weapon? EquippedRanged { get; set; }
    public Armor? EquippedArmor { get; set; }
    public Item? EquippedHead { get; set; }
    public Item? EquippedWaist { get; set; }
    public Item? EquippedHands { get; set; }
    public Item? EquippedFeet { get; set; }

    public ICollection<Item> EquippedOnBack { get; set; } = [];
    public ICollection<Item> EquippedNecklaces { get; set; } = [];
    public ICollection<Item> EquippedRings { get; set; } = [];
    public int RingCap { get; set; } = 2;
    public int NecklaceCap { get; set; } = 1;
    public int BackEquipmentCap { get; set; } = 1;

    public Item? EquippedArcaneFocus { get; set; }
    public Item? EquippedHolySymbol { get; set; }
}

[Owned]
public class Currency
{
    public int Id { get; set; }
    public int Brass { get; set; }
    public int Copper { get; set; }
    public int Silver { get; set; }
    public int Gold { get; set; }
    public int Platinum { get; set; }
    public int Electrum { get; set; }
}