using System.Text.Json.Serialization;

namespace DndWebApp.Api.Models.ExternalDTOs;

public class EFeatDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required List<string> Description { get; set; }

    [JsonPropertyName("has_prerequisite")]
    public required bool HasPrerequisite { get; set; }

    [JsonPropertyName("prerequisite")]
    public string? Prerequisite { get; set; }

    [JsonPropertyName("benefits")]
    public List<string> Benefits { get; set; } = [];
}