using System.Text.Json.Serialization;

namespace Api.Models.DTOs.ExternalDTOs;

public class ECreateItemRequestDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required List<string> Description { get; set; }

    [JsonPropertyName("equipment_category")]
    public EIndexDto? EquipmentCategory { get; set; }

    [JsonPropertyName("stack_quantity")]
    public int StackQuantity { get; set; } = 1;

    [JsonPropertyName("cost")]
    public required EQuantityDto Cost { get; set; }

    [JsonPropertyName("weight")]
    public required int Weight { get; set; }

    [JsonPropertyName("properties")]
    public List<EIndexDto>? Properties { get; set; }

    [JsonPropertyName("contents")]
    public List<EIndexDto>? Contents { get; set; }
}

public class ECreateArmorRequestDto : ECreateItemRequestDto
{
    [JsonPropertyName("armor_category")]
    public required string ArmorCategory { get; set; }

    [JsonPropertyName("armor_class")]
    public required EArmorClassDto ArmorClass { get; set; }

    [JsonPropertyName("str_minimum")]
    public required int StrengthMinimum { get; set; }

    [JsonPropertyName("stealth_disadvantage")]
    public required bool StealthDisadvantage { get; set; }
}

public class EMount : ECreateItemRequestDto
{
    [JsonPropertyName("speed")]
    public required EQuantityDto Speed { get; set; }

    [JsonPropertyName("carrying_capacity")]
    public required string CarryingCapacity { get; set; }
}

public class ECreateWeaponRequestDto : ECreateItemRequestDto
{
    [JsonPropertyName("category_range")]   
    public required string CategoryRange { get; set; }

    [JsonPropertyName("damage")]
    public EDamageDto? Damage { get; set; }

    [JsonPropertyName("range")]
    public ERangeDto? Range { get; set; }

    [JsonPropertyName("throw_range")]
    public ERangeDto? ThrowRange { get; set; }
}

public class EVehicleDto : ECreateItemRequestDto
{
    [JsonPropertyName("speed")]
    public EQuantityDto? Speed { get; set; }

    [JsonPropertyName("capacity")]
    public string? Capacity { get; set; }
}

public class ECreateToolRequestDto : ECreateItemRequestDto
{
    [JsonPropertyName("tool_category")]
    public required string ToolCategory { get; set; }
}

public class EArmorClassDto
{
    [JsonPropertyName("base")]
    public required int BaseArmorClass { get; set; }

    [JsonPropertyName("dex_bonus")]
    public required bool DexBonus { get; set; }
    
    [JsonPropertyName("max_bonus")]
    public int? MaxBonus { get; set; }

}

public class EQuantityDto
{
    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }
    
    [JsonPropertyName("unit")]
    public required string Unit { get; set; }
}

public class EDamageDto
{
    [JsonPropertyName("damage_dice")]
    public required string DamageDice { get; set; }

    [JsonPropertyName("damage_type")]
    public required EIndexDto DamageType { get; set; }
}

public class ERangeDto
{
    [JsonPropertyName("normal")]
    public required int Normal { get; set; }

    [JsonPropertyName("long")]
    public int? Long { get; set; }
}