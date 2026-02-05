using Api.Models.Items.Constants;
using Api.Services.Util.Interfaces;

namespace Api.Models.Items;

public class Weapon : Item, IEquippable
{
    public required string WeaponCategory { get; set; }
    public required string WeaponType { get; set; }
    public required string Slot { get; set; }
    public string MainSlot => Slot;
    public string? SecondarySlot => Slot == EquipSlot.MainHand ? EquipSlot.OffHand : null;
    public required ICollection<string> Properties { get; set; }
    public required ICollection<string> DamageTypes { get; set; }
    public required string DamageDice { get; set; }
    public required int Range { get; set; }
    public string? VersatileDamageDice { get; set; } = "";
    public int? LongRange { get; set; }
}