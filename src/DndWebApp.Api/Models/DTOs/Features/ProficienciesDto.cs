namespace DndWebApp.Api.Models.DTOs.Features;

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
    public required string AffinityType { get; set; }
    public required string DamageType { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class SaveThrowProficiencyDto
{
    public required string AbilityType { get; set; }
    public int AbilityId { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class SkillProficiencyDto
{
    public required string SkillType { get; set; }
    public int SkillId { get; set; }
    public required bool HasExpertise { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class WeaponCategoryProficiencyDto
{
    public required string WeaponCategory { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class WeaponTypeProficiencyDto
{
    public required string WeaponType { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class ArmorProficiencyDto
{
    public required string ArmorType { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class ToolProficiencyDto
{
    public required string ToolType { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class LanguageProficiencyDto
{
    public required string LanguageType { get; set; }
    public int LanguageId { get; set; }
    public required int CharacterFeatureId { get; set; }
}