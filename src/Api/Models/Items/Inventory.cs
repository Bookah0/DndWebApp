using Microsoft.EntityFrameworkCore;

namespace Api.Models.Items;

[Owned] // by character
public class Inventory
{
    public required Currency Currency { get; set; }
    public int TotalWeight { get; set; }
    public int MaxWeight { get; set; }
    public int AttunedItems { get; set; } = 0;
    public ICollection<InventoryItem> StoredItems { get; set; } = [];
    public ICollection<EquipmentSlot> EquipmentSlots { get; set; } = [];
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

[Owned]
public class EquipmentSlot
{
    public Item? Equipment { get; set; }
    public int? EquipmentId { get; set; }
    public required string Slot { get; set; }
}

[Owned]
public class InventoryItem
{
    public required Item Item { get; set; }
    public required int ItemId { get; set; }
    public required int Quantity { get; set; } = 1;
}