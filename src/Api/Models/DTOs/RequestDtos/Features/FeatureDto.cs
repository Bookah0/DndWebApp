using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Models.DTOs.Features;


public class FeatureDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Required]
    public required bool IsHomebrew { get; set; }

    public List<int> SpellIds { get; set; } = [];

    public List<AbilityValueDto> AbilityIncreases { get; set; } = [];

    public ProficienciesDto? Proficiencies { get; set; }

    public ProficiencyChoicesDto? ProficiencyChoices { get; set; }
}

public class FeatDto : FeatureDto
{
    [MinLength(1)]
    [MaxLength(500)]
    public string Prerequisite { get; set; } = "";

    [Required]
    [Range(1, int.MaxValue)]
    public int? FromClassId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int? FromRaceId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int? FromBackgroundId { get; set; }
}

public class BackgroundFeatureDto : FeatureDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int BackgroundId { get; set; }
}

public class ClassFeatureDto : FeatureDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int LevelId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ClassId { get; set; }
}

public class TraitDto : FeatureDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int RaceId { get; set; }
}
