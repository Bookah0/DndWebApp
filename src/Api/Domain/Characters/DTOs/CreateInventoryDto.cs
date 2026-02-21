using Api.Domain.Characters.Models;

namespace Api.Domain.Characters.DTOs;

public class CreateInventoryDto
{   public Currency Currency { get; set; } = new Currency();
    public ICollection<int> ItemIds { get; set; } = [];
    public int RingCap { get; set; } = 2;
    public int NecklaceCap { get; set; } = 1;
    public int BackEquipmentCap { get; set; } = 1;
}