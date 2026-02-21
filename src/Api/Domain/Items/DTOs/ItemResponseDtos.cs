namespace Api.Domain.Items.DTOs;

public class ItemResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ICollection<string> Categories { get; set; }
    public string? Rarity { get; set; }
    public bool RequiresAttunement { get; set; }
    public int Weight { get; set; }
    public int Value { get; set; }
    public int Quantity { get; set; }
    public string? EquipSlot { get; set; }
    public string? SecondaryEquipSlot { get; set; }
}

public class WeaponResponseDto : ItemResponseDto
{
    public required string WeaponCategory { get; set; }
    public required string WeaponType { get; set; }
    public required ICollection<string> Properties { get; set; }
    public required ICollection<string> DamageTypes { get; set; }
    public required string DamageDice { get; set; }
    public required int Range { get; set; }
    public string? VersitileDamageDice { get; set; }
    public int? LongRange { get; set; }
}

public class ArmorResponseDto : ItemResponseDto
{
    public required string ArmorCategory { get; set; }
    public required int BaseArmorClass { get; set; }
    public required bool PlusDexMod { get; set; }
    public int? ModCap { get; set; }
    public int? StrengthScoreRequired { get; set; }
    public bool StealthDisadvantage { get; set; }
}

public class ToolResponseDto : ItemResponseDto
{
    public required string ToolCategory { get; set; }
    public required ICollection<ToolPropertyDto> Properties { get; set; }
    public ICollection<ToolActivityDto> Activities { get; set; } = [];
}

public class EquippedItemDto : ItemResponseDto
{
    public string? WeaponCategory { get; set; }
    public string? WeaponType { get; set; }
    public ICollection<string>? Properties { get; set; }
    public ICollection<string>? DamageTypes { get; set; }
    public string? DamageDice { get; set; }
    public int? Range { get; set; }
    public string? VersitileDamageDice { get; set; }
    public int? LongRange { get; set; }
    public string? ArmorCategory { get; set; }
    public int? BaseArmorClass { get; set; }
    public bool? PlusDexMod { get; set; }
    public int? ModCap { get; set; }
    public int? StrengthScoreRequired { get; set; }
    public bool? StealthDisadvantage { get; set; }
}