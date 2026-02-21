using System.ComponentModel.DataAnnotations;
using Api.Domain.Abilities.DTOs;

namespace Api.Domain.Shared.DTOs;

public class ProficiencyChoicesDto
{
    public int Id { get; set; }
    public ICollection<AbilityIncreaseChoiceDto> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyChoiceDto> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyChoiceDto> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageProficiencyChoiceDto> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyChoiceDto> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponCategoryProficiencyChoiceDto> WeaponCategoryProficiencyChoices { get; set; } = [];
    public ICollection<WeaponTypeProficiencyChoiceDto> WeaponTypeProficiencyChoices { get; set; } = [];
}

public abstract class ChoiceDto
{
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }
}

public class ItemChoiceDto : ChoiceDto
{
    [Range(1, int.MaxValue)]
    public required int NumberOfOptions { get; set; }

    public required ICollection<int> IdOptions { get; set; }
}

public class LanguageProficiencyChoiceDto : ChoiceDto
{
    public required ICollection<int> IdOptions { get; set; }
}

public class SkillProficiencyChoiceDto : ChoiceDto
{
    public required ICollection<int> IdOptions { get; set; }
}

public class AbilityIncreaseChoiceDto : ChoiceDto
{
    public required ICollection<AbilityValueDto> ValueOptions { get; set; }
}

public class WeaponCategoryProficiencyChoiceDto : ChoiceDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public class WeaponTypeProficiencyChoiceDto : ChoiceDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ToolProficiencyChoiceDto : ChoiceDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ArmorProficiencyChoiceDto : ChoiceDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public abstract class AOptionDto
{
    [Range(1, int.MaxValue)]
    public required int ChoiceId { get; set; }
}

public class LanguageOptionDto : AOptionDto
{
    public required ICollection<int> IdOptions { get; set; }
}

public class SkillOptionDto : AOptionDto
{
    public required ICollection<int> IdOptions { get; set; }
}

public class AbilityValueOptionDto : AOptionDto
{
    public required ICollection<AbilityValueDto> ValueOptions { get; set; }
}

public class WeaponCategoryOptionDto : AOptionDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public class WeaponTypeOptionDto : AOptionDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ToolOptionDto : AOptionDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ArmorOptionDto : AOptionDto
{
    public required ICollection<string> ConstantsOption { get; set; }
}

public class StartingEquipmentOptionDto : AOptionDto
{
    public required int EquipmentId { get; set; }
    public string? AnyOfArmorCategory { get; set; }
    public string? AnyOfWeaponCategory { get; set; }
    public string? AnyOfWeaponType { get; set; }
    public int Quantity { get; set; } = 1;
}