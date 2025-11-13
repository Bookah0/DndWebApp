using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.DTOs.ExternalDtos;

public class ESkillDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("desc")]
    public required List<string> Description { get; set; }

    [JsonPropertyName("ability_score")]
    public required EIndexDto Ability { get; set; }
}