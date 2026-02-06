using System.Text.Json.Serialization;

namespace Api.Models.DTOs.ExternalDTOs;

public class EOpen5eResponseDto<T> where T : class
{
    [JsonPropertyName("count")]
    public required int Count { get; set; }

    [JsonPropertyName("next")]
    public required string Next { get; set; }

    [JsonPropertyName("previous")]
    public required string Previous { get; set; }

    [JsonPropertyName("results")]
    public required ICollection<T> Results { get; set; }
}