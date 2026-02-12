using System.Text.Json.Serialization;

namespace Api.Models.DTOs.ExternalDTOs;

public class EIndexListDto
{
    [JsonPropertyName("results")]
    public required List<EIndexDto> Results { get; set; }
}