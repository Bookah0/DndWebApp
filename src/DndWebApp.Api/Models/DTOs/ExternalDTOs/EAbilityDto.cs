using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class EAbilityDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("full_name")]
    public required string FullName { get; set; }
    
    [JsonPropertyName("desc")]
    public required List<string> Description { get; set; }
    
    [JsonPropertyName("skills")]
    public required List<EIndexDto> Skills { get; set; }
}