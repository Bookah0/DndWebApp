using Api.Validation.AllowedValues.Items;
using Api.Services.Util.Interfaces;

namespace Api.Models.Items;

public class Armor : Item, IEquippable
{
    public required string ArmorCategory { get; set; }
    public required int BaseArmorClass { get; set; }
    public required bool PlusDexMod { get; set; }
    public int? ModCap { get; set; }
    public int? StrengthScoreRequired { get; set; }
    public bool StealthDisadvantage { get; set; } = false;
    public string MainSlot => ArmorCategory.Equals(Validation.AllowedValues.Items.ArmorCategory.Shield) ? EquipSlot.OffHand : EquipSlot.Armor;
    public string? SecondarySlot => null;
}