using Api.Domain.Items.DTOs;
using Api.Domain.Items.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Characters.DTOs;

public class InventoryResponseDto
{
public int Id { get; set; }
    public int CharacterId { get; set; }
    public required CurrencyDto Currency { get; set; }
    public int TotalWeight { get; set; }
    public int MaxWeight { get; set; }
    public int AttunedItems { get; set; } = 0;
    public ICollection<InventoryItemResponseDto> StoredItems { get; set; } = [];
    public ICollection<EquipmentSlotDto> EquipmentSlots { get; set; } = [];
}

public class EquipmentSlotDto
{
    public required Item Equipment { get; set; }
    public required string Slot { get; set; }
}

public class InventoryItemResponseDto
{
    public required int Quantity { get; set; }
    public required ItemResponseDto Item { get; set; }
}