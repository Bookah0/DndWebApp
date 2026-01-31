using System.ComponentModel.DataAnnotations;
using DndWebApp.Api.Models.DTOs.Character;

namespace DndWebApp.Api.Models.DTOs.Features;

public class ProficiencyChoicesDto
{    public ICollection<AbilityIncreaseOptionDto> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyOptionDto> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyOptionDto> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageOptionDto> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyOptionDto> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponProficiencyOptionDto> WeaponProficiencyChoices { get; set; } = [];
}

public class SkillProficiencyOptionDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<string> Options { get; set; }
}

public class AbilityIncreaseOptionDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<AbilityValueDto> Options { get; set; }
}

public class ToolProficiencyOptionDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<string> Options { get; set; }
}

public class LanguageOptionDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<string> Options { get; set; }
}

public class WeaponProficiencyOptionDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    public ICollection<string> CategoryOptions { get; set; } = [];
    public ICollection<string> TypeOptions { get; set; } = [];
}

public class ArmorProficiencyOptionDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    [Required]
    public required ICollection<string> Options { get; set; }
}