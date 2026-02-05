using System.ComponentModel.DataAnnotations;

public class ProficienciesDto
{
    public ICollection<ProficiencyDto> SavingThrowProficiencies { get; set; } = [];
    public ICollection<ProficiencyDto> SkillProficiencies { get; set; } = [];
    public ICollection<ProficiencyDto> WeaponCategoryProficiencies { get; set; } = [];
    public ICollection<ProficiencyDto> WeaponTypeProficiencies { get; set; } = [];
    public ICollection<ProficiencyDto> ArmorProficiencies { get; set; } = [];
    public ICollection<ProficiencyDto> ToolProficiencies { get; set; } = [];
    public ICollection<ProficiencyDto> Languages { get; set; } = [];
    public ICollection<ProficiencyDto> DamageAffinities { get; set; } = [];
}

public class ProficiencyDto
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