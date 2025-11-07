using DndWebApp.Api.Models.Items.Enums;
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
    public ICollection<Item> StoredItems { get; set; } = [];
    public ICollection<EquipmentSlot> EquippedItems { get; set; } = [];
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
    public int? EquipmentId { get; set; }
    public required EquipSlot Slot { get; set; }
}