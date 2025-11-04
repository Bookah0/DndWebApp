using DndWebApp.Api.Models.Items.Enums;

namespace DndWebApp.Api.Models.DTOs;

public class WeaponDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int Weight { get; set; }
    public required int Value { get; set; }
    public required string WeaponCategory { get; set; }
    public required string WeaponType { get; set; }
    public required string DamageDice { get; set; }
    public required int Range { get; set; }
    public required string MainDamageType { get; set; }
    public ICollection<string> OtherDamageTypes { get; set; } = [];
    public ICollection<string> Properties { get; set; } = [];
    public string? VersitileDamageDice { get; set; }
    public int? LongRange { get; set; }
    public string? Rarity { get; set; }
    public bool? RequiresAttunement { get; set; }
    public bool? IsHomebrew { get; set; }
}