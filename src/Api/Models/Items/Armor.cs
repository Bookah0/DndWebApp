using Api.Validation.AllowedValues.Items;
using Api.Services.Util.Interfaces;

namespace Api.Models.Items;

public class Armor : Item
{
    public required string ArmorCategory { get; set; }
    public required int BaseArmorClass { get; set; }
    public required bool PlusDexMod { get; set; }
    public int? ModCap { get; set; }
    public int? StrengthScoreRequired { get; set; }
    public bool StealthDisadvantage { get; set; } = false;
}