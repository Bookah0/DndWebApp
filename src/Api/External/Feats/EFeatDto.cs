using System.Text.Json.Serialization;

namespace Api.External.Feats;

public class EFeatDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }

    [JsonPropertyName("prerequisite")]
    public string? Prerequisite { get; set; }

    [JsonPropertyName("effects_desc")]
    public List<string> EffectsDescriptions { get; set; } = [];
}