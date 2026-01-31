using System.ComponentModel.DataAnnotations;
using DndWebApp.Api.Models.DTOs.Character;

namespace DndWebApp.Api.Models.DTOs.Features;

public class ClassFeatureDto
{
    public int Id { get; set; }

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

    [Required]
    [Range(1, int.MaxValue)]
    public required int ClassLevelId { get; set; }

    public List<int> SpellIds { get; set; } = [];

    public List<AbilityValueDto> AbilityIncreases { get; set; } = [];

    public ProficienciesDto? Proficiencies { get; set; }

    public List<ProficiencyDto> DamageAffinities { get; set; } = [];

    public ProficiencyChoicesDto? ProficiencyChoices { get; set; }
}

