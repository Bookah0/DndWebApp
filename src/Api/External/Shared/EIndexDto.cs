using System.Text.Json.Serialization;

namespace Api.External.Shared;

public class EIndexDto
{
    [JsonPropertyName("index")]
    public required string Index { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("url")]
    public required string Url { get; set; }
}