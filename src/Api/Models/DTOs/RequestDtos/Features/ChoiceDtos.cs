using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.Features;
using Api.Models.Items;

namespace Api.Models.DTOs.Features;

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
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }
}

public class ItemChoiceDto : ChoiceDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int NumberOfOptions { get; set; }

    [Required]
    public required ICollection<int> IdOptions { get; set; }
}

public class LanguageProficiencyChoiceDto : ChoiceDto
{
    [Required]
    public required ICollection<int> IdOptions { get; set; }
}

public class SkillProficiencyChoiceDto : ChoiceDto
{
    [Required]
    public required ICollection<int> IdOptions { get; set; }
}

public class AbilityIncreaseChoiceDto : ChoiceDto
{
    [Required]
    public required ICollection<AbilityValueDto> ValueOptions { get; set; }
}

public class WeaponCategoryProficiencyChoiceDto : ChoiceDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class WeaponTypeProficiencyChoiceDto : ChoiceDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ToolProficiencyChoiceDto : ChoiceDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ArmorProficiencyChoiceDto : ChoiceDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public abstract class AOptionDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int ChoiceId { get; set; }
}

public class LanguageOptionDto : AOptionDto
{
    [Required]
    public required ICollection<int> IdOptions { get; set; }
}

public class SkillOptionDto : AOptionDto
{
    [Required]
    public required ICollection<int> IdOptions { get; set; }
}

public class AbilityValueOptionDto : AOptionDto
{
    [Required]
    public required ICollection<AbilityValueDto> ValueOptions { get; set; }
}

public class WeaponCategoryOptionDto : AOptionDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class WeaponTypeOptionDto : AOptionDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ToolOptionDto : AOptionDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ArmorOptionDto : AOptionDto
{
    [Required]
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