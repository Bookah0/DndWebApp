using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.ExternalDTOs;

public class EIndexDto
{
    [JsonPropertyName("index")]
    public required string Index { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("url")]
    public required string Url { get; set; }
}