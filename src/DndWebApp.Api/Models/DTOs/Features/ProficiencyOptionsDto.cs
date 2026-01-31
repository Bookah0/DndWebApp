using System.ComponentModel.DataAnnotations;
using DndWebApp.Api.Models.DTOs.Character;

namespace DndWebApp.Api.Models.DTOs.Features;

public class ProficiencyChoicesDto
{
    public int Id { get; set; }
    public ICollection<AbilityIncreaseChoiceDto> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<ProficiencyChoiceDto> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ProficiencyChoiceDto> ToolProficiencyChoices { get; set; } = [];
    public ICollection<ProficiencyChoiceDto> LanguageChoices { get; set; } = [];
    public ICollection<ProficiencyChoiceDto> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<ProficiencyChoiceDto> WeaponProficiencyChoices { get; set; } = [];
}


public class ProficiencyChoiceDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Type { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<string> Options { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public required int FeatureId { get; set; }
}

public class AbilityIncreaseChoiceDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<AbilityValueDto> Options { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int FeatureId { get; set; }
}