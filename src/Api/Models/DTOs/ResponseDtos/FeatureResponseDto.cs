namespace Api.Models.DTOs.ResponseDtos;

public class FeatureResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; }
    public ICollection<AbilityValueResponseDto> AbilityIncreases { get; set; } = [];
    public ICollection<SpellResponseDto> SpellsGained { get; set; } = [];
    public ProficienciesResponseDto? Proficiencies { get; set; }
    public ProficiencyChoicesResponseDto? ProficiencyChoices { get; set; }
}

public class FeatResponseDto : FeatureResponseDto
{
    public string Prerequisite { get; set; } = "";
    public int? FromClassId { get; set; }
    public int? FromRaceId { get; set; }
    public int? FromBackgroundId { get; set; }
}

public class BackgroundFeatureResponseDto : FeatureResponseDto
{
    public required int BackgroundId { get; set; }
}

public class ClassFeatureResponseDto : FeatureResponseDto
{
    public required int ClassLevelId { get; set; }
}

public class TraitResponseDto : FeatureResponseDto
{
    public required int RaceId { get; set; }
}

public class AbilityValueResponseDto
{
    public required string AbilityType { get; set; }
    public required int Increase { get; set; }
}