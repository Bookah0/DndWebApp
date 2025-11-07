using DndWebApp.Api.Models.Items.Enums;

namespace DndWebApp.Api.Models.DTOs;

public class ArmorDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Weight { get; set; }
    public int Value { get; set; }
    public required string Category { get; set; }
    public required int BaseArmorClass { get; set; }
    public required bool PlusDexMod { get; set; }
    public string? Rarity { get; set; }
    public bool? RequiresAttunement { get; set; }
    public bool? IsHomebrew { get; set; }
    public int? ModCap { get; set; }
    public int? StrengthScoreRequired { get; set; }
    public bool? StealthDisadvantage { get; set; }
}