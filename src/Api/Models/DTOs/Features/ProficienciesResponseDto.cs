namespace Api.Models.DTOs.ResponseDtos;

public class ProficienciesResponseDto
{
    public ICollection<SaveThrowProficiencyResponseDto> SavingThrowProficiencies { get; set; } = [];
    public ICollection<SkillProficiencyResponseDto> SkillProficiencies { get; set; } = [];
    public ICollection<WeaponCategoryProficiencyResponseDto> WeaponCategoryProficiencies { get; set; } = [];
    public ICollection<WeaponTypeProficiencyResponseDto> WeaponTypeProficiencies { get; set; } = [];
    public ICollection<ArmorProficiencyResponseDto> ArmorProficiencies { get; set; } = [];
    public ICollection<ToolProficiencyResponseDto> ToolProficiencies { get; set; } = [];
    public ICollection<LanguageProficiencyResponseDto> Languages { get; set; } = [];
    public ICollection<DamageAffinityResponseDto> DamageAffinities { get; set; } = [];
}

public class DamageAffinityResponseDto
{
    public required string AffinityType { get; set; }
    public required string DamageType { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class SaveThrowProficiencyResponseDto
{
    public required string AbilityType { get; set; }
    public int AbilityId { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class SkillProficiencyResponseDto
{
    public required string SkillType { get; set; }
    public int SkillId { get; set; }
    public required bool HasExpertise { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class WeaponCategoryProficiencyResponseDto
{
    public required string WeaponCategory { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class WeaponTypeProficiencyResponseDto
{
    public required string WeaponType { get; set; }

    public required int CharacterFeatureId { get; set; }
}

public class ArmorProficiencyResponseDto
{
    public required string ArmorType { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class ToolProficiencyResponseDto
{
    public required string ToolType { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class LanguageProficiencyResponseDto
{
    public required string LanguageType { get; set; }
    public int LanguageId { get; set; }
    public required int CharacterFeatureId { get; set; }
}

public class ProficiencyChoicesResponseDto
{    public ICollection<AbilityIncreaseChoiceResponseDto> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyChoiceResponseDto> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyChoiceResponseDto> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageChoiceResponseDto> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyChoiceResponseDto> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponProficiencyChoiceResponseDto> WeaponProficiencyChoices { get; set; } = [];
}

public class SkillProficiencyChoiceResponseDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}

public class AbilityIncreaseChoiceResponseDto
{
    public required string Description { get; set; }
    public required ICollection<AbilityValueResponseDto> Options { get; set; }
}

public class ToolProficiencyChoiceResponseDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}

public class LanguageChoiceResponseDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}

public class WeaponProficiencyChoiceResponseDto
{
    public required string Description { get; set; }
    public ICollection<string> CategoryOptions { get; set; } = [];
    public ICollection<string> TypeOptions { get; set; } = [];
}

public class ArmorProficiencyChoiceResponseDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}