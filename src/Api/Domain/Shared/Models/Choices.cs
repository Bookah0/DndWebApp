using Api.Domain.Abilities.Models;
using Api.Domain.Languages.Models;
using Api.Domain.Shared.Interfaces;
using Api.Domain.Skills.Models;

namespace Api.Domain.Shared.Models;

public class AbilityIncreaseChoice : IFeatureChoice
{
    public  int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<AbilityValue> Options { get; set; }
    public int FeatureId { get; set; }
}

public class SkillProficiencyChoice : IFeatureChoice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<Skill> Options { get; set; }
    public int FeatureId { get; set; }
}

public class ToolProficiencyChoice : IFeatureChoice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
    public int FeatureId { get; set; }
}

public class LanguageChoice : IFeatureChoice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<Language> Options { get; set; }
    public int FeatureId { get; set; }
}

public class WeaponCategoryProficiencyChoice : IFeatureChoice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
    public int FeatureId { get; set; }
}

public class WeaponTypeProficiencyChoice : IFeatureChoice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
    public int FeatureId { get; set; }
}

public class ArmorProficiencyChoice : IFeatureChoice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<string> Options { get; set; }
    public int FeatureId { get; set; }
}