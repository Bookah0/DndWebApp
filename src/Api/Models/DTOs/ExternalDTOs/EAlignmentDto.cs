using System.Text.Json.Serialization;

namespace Api.Models.DTOs.ExternalDTOs;

public class EAlignmentDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("abbreviation")]
    public required string Abbreviation { get; set; }
    
    [JsonPropertyName("desc")]
    public required string Description { get; set; }
}