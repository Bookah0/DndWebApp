using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class ELanguageDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }
    
    [JsonPropertyName("typical_speakers")]
    public required List<string> TypicalSpeakers { get; set; }
    
    [JsonPropertyName("script")]
    public required string Script { get; set; }
}