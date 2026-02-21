using System.Text.Json.Serialization;

namespace Api.External.Languages;

public class ELanguageDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }
    
    [JsonPropertyName("typical_speakers")]
    public required List<string> TypicalSpeakers { get; set; }
    
    [JsonPropertyName("script")]
    public string? Script { get; set; }
}