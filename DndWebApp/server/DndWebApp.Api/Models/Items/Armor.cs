using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Items.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Items;

public class Armor : Item
{
    public required ArmorCategory Category { get; set; }
    public required int BaseArmorClass { get; set; }
    public required bool PlusDexMod { get; set; }
    public int ModCap { get; set; } = 0;
    public int? StrengthScoreRequired { get; set; }
    public bool StealthDisadvantage { get; set; } = false;
}

[Owned]
public class ArmorProficiency
{
    public required ArmorCategory ArmorType { get; set; }
    public required int CharacterFeatureId { get; set; }
}