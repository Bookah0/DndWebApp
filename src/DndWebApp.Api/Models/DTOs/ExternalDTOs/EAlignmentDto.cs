using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EAlignmentDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("abbreviation")]
    public required string Abbreviation { get; set; }
    
    [JsonPropertyName("desc")]
    public required string Description { get; set; }
}