using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.ExternalDTOs;

public class EFeatListDto
{
    [JsonPropertyName("results")]
    public required List<EFeatDto> Results { get; set; }
}