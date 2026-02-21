namespace Api.Domain.Shared.Enums.Character;

public class ProficiencyType : IValuesProvider
{
    public const string Skill = "Skill";
    public const string WeaponCategory = "Weapon Category";
    public const string WeaponType = "Weapon Type";
    public const string ArmorCategory = "Armor Category";
    public const string ToolCategory = "Tool Category";
    public const string Language = "Language";
    public const string AbilityScore = "Ability Score";
    public const string AbilityIncrease = "Ability Increase";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Skill, WeaponCategory, WeaponType, ArmorCategory, ToolCategory, Language, AbilityScore, AbilityIncrease
    };  

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}