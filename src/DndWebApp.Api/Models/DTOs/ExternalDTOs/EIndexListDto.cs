using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EIndexListDto
{
    [JsonPropertyName("results")]
    public required List<EIndexDto> Results { get; set; }
}