using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Shared.DTOs;

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
    [MinLength(1)]
    [MaxLength(100)]
    public required string Type { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public required string Value { get; set; }
}