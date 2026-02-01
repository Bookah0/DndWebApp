using DndWebApp.Api.Models.DTOs.Inventory;

namespace DndWebApp.Api.Models.DTOs.ResponseDtos;

public class InventoryResponseDto
{
public int Id { get; set; }
    public int CharacterId { get; set; }
    public required CurrencyDto Currency { get; set; }
    public int TotalWeight { get; set; }
    public int MaxWeight { get; set; }
    public int AttunedItems { get; set; } = 0;
    public ICollection<ItemResponseDto> StoredItems { get; set; } = [];
    public ICollection<EquipmentSlotDto> EquippedItems { get; set; } = [];
}

public class EquipmentSlotDto
{
    public int? EquipmentId { get; set; }
    public required string Slot { get; set; }
}