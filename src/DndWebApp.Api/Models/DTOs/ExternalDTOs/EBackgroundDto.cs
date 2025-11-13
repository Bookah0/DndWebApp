using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EBackgroundDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }

    [JsonPropertyName("benefits")]
    public List<EBackgroundBenefitDto> Benefits { get; set; } = [];

}

public class EBackgroundBenefitDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }
}
