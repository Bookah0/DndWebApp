namespace Dndtoolkit.Api.Models.DTOs.RequestDtos.Features;

using System.ComponentModel.DataAnnotations;

public class ProficienciesRequestDto
{
    public ICollection<ProficiencyRequestDto> SavingThrowProficiencies { get; set; } = [];
    public ICollection<ProficiencyRequestDto> SkillProficiencies { get; set; } = [];
    public ICollection<ProficiencyRequestDto> WeaponCategoryProficiencies { get; set; } = [];
    public ICollection<ProficiencyRequestDto> WeaponTypeProficiencies { get; set; } = [];
    public ICollection<ProficiencyRequestDto> ArmorProficiencies { get; set; } = [];
    public ICollection<ProficiencyRequestDto> ToolProficiencies { get; set; } = [];
    public ICollection<ProficiencyRequestDto> Languages { get; set; } = [];
    public ICollection<ProficiencyRequestDto> DamageAffinities { get; set; } = [];
}

public class ProficiencyRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Type { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Value { get; set; }
}