using System.Text.Json.Serialization;
using DndWebApp.Api.Models.ExternalDTOs;

public class ESpeciesDto
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("ability_bonuses")]
    public required List<EAbilityBonusDto> AbilityBonuses { get; set; }

    [JsonPropertyName("traits")]
    public required List<EIndexDto> Traits { get; set; }
}

public class ERaceDto : ESpeciesDto
{
    [JsonPropertyName("speed")]
    public required int Speed { get; set; }

    [JsonPropertyName("alignment")]
    public required string PreferedAlignment { get; set; }

    [JsonPropertyName("age")]
    public required string Age { get; set; }

    [JsonPropertyName("size")]
    public required string Size { get; set; }

    [JsonPropertyName("size_description")]
    public required string SizeDescription { get; set; }

    [JsonPropertyName("languages")]
    public required List<EIndexDto> StartingLanguages { get; set; }

    [JsonPropertyName("language_desc")]
    public required string AvailableLanguagesDescription { get; set; }

    [JsonPropertyName("subraces")]
    public required List<EIndexDto> Subraces { get; set; }
}

public class ESubraceDto : ESpeciesDto
{
    [JsonPropertyName("race")]
    public required EIndexDto FromRace { get; set; }

    [JsonPropertyName("desc")]
    public required string Description { get; set; }
}

public class EAbilityBonusDto
{
    [JsonPropertyName("ability_score")]
    public required EIndexDto AbilityScore { get; set; }

    [JsonPropertyName("bonus")]
    public required int Bonus { get; set; }
}