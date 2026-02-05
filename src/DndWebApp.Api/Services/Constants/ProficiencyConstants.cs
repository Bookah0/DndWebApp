namespace DndWebApp.Api.Services.Constants;

public static class ProficiencyConstants
{
    public const string Skill = "Skill";
    public const string WeaponCategory = "Weapon Category";
    public const string WeaponType = "Weapon Type";
    public const string ArmorCategory = "Armor Category";
    public const string ToolCategory = "Tool Category";
    public const string Language = "Language";
    public const string AbilityScore = "Ability Score";
    public const string AbilityIncrease = "Ability Increase";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Skill, WeaponCategory, WeaponType, ArmorCategory, ToolCategory, Language, AbilityScore, AbilityIncrease
    };
}