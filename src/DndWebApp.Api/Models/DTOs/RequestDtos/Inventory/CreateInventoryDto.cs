using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Models.DTOs.Inventory;

public class CreateInventoryDto
{
    public int Id { get; set; }
    public int CopperCoins { get; set; } = 0;
    public required int CharacterId { get; set; }
    public List<int> ItemIds { get; set; } = [];
    public int RingCap { get; set; } = 2;
    public int NecklaceCap { get; set; } = 1;
    public int BackEquipmentCap { get; set; } = 1;
}