namespace Api.Models.DTOs.ExternalDTOs;

public class EMagicCreateItemRequestDto
{
    public required ECreateItemRequestDto Item { get; set; }
    public required ERarityDto Rarity { get; set; }
}

public class ERarityDto
{
    public required string Name { get; set; }
}