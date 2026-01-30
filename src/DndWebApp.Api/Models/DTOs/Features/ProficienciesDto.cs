using System.ComponentModel.DataAnnotations;

public class ProficienciesDto
{
    public ICollection<SaveThrowProficiencyDto> SavingThrowProficiencies { get; set; } = [];
    public ICollection<SkillProficiencyDto> SkillProficiencies { get; set; } = [];
    public ICollection<WeaponCategoryProficiencyDto> WeaponCategoryProficiencies { get; set; } = [];
    public ICollection<WeaponTypeProficiencyDto> WeaponTypeProficiencies { get; set; } = [];
    public ICollection<ArmorProficiencyDto> ArmorProficiencies { get; set; } = [];
    public ICollection<ToolProficiencyDto> ToolProficiencies { get; set; } = [];
    public ICollection<LanguageProficiencyDto> Languages { get; set; } = [];
    public ICollection<DamageAffinityDto> DamageAffinities { get; set; } = [];
}

public class DamageAffinityDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string AffinityType { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string DamageType { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}

public class SaveThrowProficiencyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string AbilityType { get; set; }

    [Range(0, int.MaxValue)]
    public int AbilityId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}

public class SkillProficiencyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string SkillType { get; set; }

    [Range(0, int.MaxValue)]
    public int SkillId { get; set; }

    [Required]
    public required bool HasExpertise { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}

public class WeaponCategoryProficiencyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string WeaponCategory { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}

public class WeaponTypeProficiencyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string WeaponType { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}

public class ArmorProficiencyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string ArmorType { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}

public class ToolProficiencyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string ToolType { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}

public class LanguageProficiencyDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string LanguageType { get; set; }

    [Range(0, int.MaxValue)]
    public int LanguageId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int CharacterFeatureId { get; set; }
}