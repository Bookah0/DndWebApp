namespace DndWebApp.Api.Models.DTOs.Features;

public class ProficiencyChoicesDto
{
    public int Id { get; set; }
    public ICollection<AbilityIncreaseOptionDto> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyOptionDto> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyOptionDto> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageOptionDto> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyOptionDto> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponProficiencyOptionDto> WeaponProficiencyChoices { get; set; } = [];
}

public class SkillProficiencyOptionDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}

public class AbilityIncreaseOptionDto
{
    public required string Description { get; set; }
    public required ICollection<AbilityValueDto> Options { get; set; }
}

public class ToolProficiencyOptionDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}

public class LanguageOptionDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}

public class WeaponProficiencyOptionDto
{    
    public required string Description { get; set; }
    public ICollection<string> CategoryOptions { get; set; } = [];
    public ICollection<string> TypeOptions { get; set; } = [];
}

public class ArmorProficiencyOptionDto
{
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
}