namespace Api.Models.DTOs.ExternalDTOs;

public class EMagicItemDto
{
    public required EItemDto Item { get; set; }
    public required ERarityDto Rarity { get; set; }
}

public class ERarityDto
{
    public required string Name { get; set; }
}