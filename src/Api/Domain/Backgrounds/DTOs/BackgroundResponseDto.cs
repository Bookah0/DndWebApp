using Api.Domain.Shared.DTOs;

namespace Api.Domain.Backgrounds.DTOs;

public class BackgroundResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ICollection<BackgroundFeatureResponseDto> Features { get; set; }
    public ICollection<int> StartingItemsIds { get; set; } = [];
    public ICollection<int> StartingItemsChoiceIds { get; set; } = [];
}