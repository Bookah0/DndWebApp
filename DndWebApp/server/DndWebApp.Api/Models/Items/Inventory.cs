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
    public ICollection<int> TreasureIds { get; set; } = [];      // Categories: Gem, Jewelry, WondrousItem, TradeGood
    public ICollection<int> EquipmentIds { get; set; } = [];     // Categories: Ammunition, Armor, Ring, Rod, Shield, SpellcastingFocus, Staff, Wand, Weapon
    public ICollection<int> ConsumablesIds { get; set; } = [];   // Categories: Poison, Potion, Scroll
    public ICollection<int> GearIds { get; set; } = [];          // Categories: AdventuringGear, EquipmentPack, Tools
    public ICollection<int> MiscIds { get; set; } = [];          // Categories: LandVehicle, Mount, WaterborneVehicle, null ItemCategory
}

[Owned]
public class EquippedItems
{
    public int? EquippedMainHandId { get; set; }
    public int? EquippedOffHandId { get; set; }
    public int? EquippedRangedId { get; set; }
    public int? EquippedArmorId { get; set; }
    public int? EquippedHeadId { get; set; }
    public int? EquippedWaistId { get; set; }
    public int? EquippedHandsId { get; set; }
    public int? EquippedFeetId { get; set; }

    public ICollection<int> EquippedRingIds { get; set; } = [];
    public int RingCap { get; set; } = 2;

    public ICollection<int> EquippedNecklaceIds { get; set; } = [];
    public int NecklaceCap { get; set; } = 1;

    public ICollection<int> EquippedOnBackIds { get; set; } = [];
    public int BackEquipmentCap { get; set; } = 1;

    public int? EquippedArcaneFocusId { get; set; }
    public int? EquippedHolySymbolId { get; set; }
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