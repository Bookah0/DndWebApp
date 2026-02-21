using System.Text.Json.Serialization;
using Api.External.Shared;

namespace Api.External.Skills;

public class ESkillDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("desc")]
    public required List<string> Description { get; set; }

    [JsonPropertyName("ability_score")]
    public required EIndexDto Ability { get; set; }
}