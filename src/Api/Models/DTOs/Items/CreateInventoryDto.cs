using Api.Models.Characters;
using Api.Models.Features;
using Api.Models.Items;

namespace Api.Models.DTOs.RequestDtos.Inventory;

public class CreateInventoryDto
{   public Currency Currency { get; set; } = new Currency();
    public ICollection<int> ItemIds { get; set; } = [];
    public int RingCap { get; set; } = 2;
    public int NecklaceCap { get; set; } = 1;
    public int BackEquipmentCap { get; set; } = 1;
}