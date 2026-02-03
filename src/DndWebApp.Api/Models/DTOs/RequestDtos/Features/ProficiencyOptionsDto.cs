using System.ComponentModel.DataAnnotations;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items;

namespace DndWebApp.Api.Models.DTOs.Features;

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

public abstract class AChoiceDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }
}

public class LanguageProficiencyChoiceDto : AChoiceDto
{
    [Required]
    public required ICollection<int> IdOptions { get; set; }
}

public class SkillProficiencyChoiceDto : AChoiceDto
{
    [Required]
    public required ICollection<int> IdOptions { get; set; }
}

public class AbilityIncreaseChoiceDto : AChoiceDto
{
    [Required]
    public required ICollection<AbilityValueDto> ValueOptions { get; set; }
}

public class WeaponCategoryProficiencyChoiceDto : AChoiceDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class WeaponTypeProficiencyChoiceDto : AChoiceDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ToolProficiencyChoiceDto : AChoiceDto
{
    [Required]
    public required ICollection<string> ConstantsOption { get; set; }
}

public class ArmorProficiencyChoiceDto : AChoiceDto
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