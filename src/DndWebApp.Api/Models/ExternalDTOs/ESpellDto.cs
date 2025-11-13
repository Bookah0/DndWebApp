namespace DndWebApp.Api.Models.ExternalDTOs;
using System.Text.Json.Serialization;

public class EOpen5eSpellDto
{
    [JsonPropertyName("slug")]
    public required string Index { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }

    [JsonPropertyName("higher_level")]
    public string? HigherLevels { get; set; }

    [JsonPropertyName("range")]
    public required string Range { get; set; }

    [JsonPropertyName("target_range_sort")]
    public required int RangeValue { get; set; }

    [JsonPropertyName("components")]
    public required string Components { get; set; }

    [JsonPropertyName("requires_verbal_components")]
    public required bool RequiresVerbalComponents { get; set; }

    [JsonPropertyName("requires_somatic_components")]
    public required bool RequiresSomaticComponents { get; set; }

    [JsonPropertyName("requires_material_components")]
    public required bool RequiresMaterialComponents { get; set; }

    [JsonPropertyName("material")]
    public string? Material { get; set; }

    [JsonPropertyName("can_be_cast_as_ritual")]
    public required bool IsRitual { get; set; }

    [JsonPropertyName("duration")]
    public required string Duration { get; set; }

    [JsonPropertyName("casting_time")]
    public required string CastingTime { get; set; }

    [JsonPropertyName("spell_level")]
    public required int SpellLevel { get; set; }

    [JsonPropertyName("school")]
    public required string School { get; set; }
}

public class EDnd5eApiSpellDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("desc")]
    public required List<string> Description { get; set; }

    [JsonPropertyName("higher_level")]
    public List<string>? HigherLevel { get; set; }

    [JsonPropertyName("range")]
    public required string Range { get; set; }

    [JsonPropertyName("components")]
    public required List<string> Components { get; set; } = [];

    [JsonPropertyName("material")]
    public string? Material { get; set; }

    [JsonPropertyName("ritual")]
    public required bool Ritual { get; set; }

    [JsonPropertyName("duration")]
    public required string Duration { get; set; }

    [JsonPropertyName("casting_time")]
    public required string CastingTime { get; set; }

    [JsonPropertyName("level")]
    public required int Level { get; set; }

    [JsonPropertyName("school")]
    public required EIndexDto School { get; set; }

    [JsonPropertyName("area_of_effect")]
    public required ESpellAoeDto Aoe { get; set; }
}

public class ESpellAoeDto
{
    [JsonPropertyName("type")]
    public required string AoeType { get; set; }

    [JsonPropertyName("size")]
    public required int AoeSize { get; set; }
}