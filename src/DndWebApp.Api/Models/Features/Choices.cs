namespace DndWebApp.Api.Models.Features;

using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;

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