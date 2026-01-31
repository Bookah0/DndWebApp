namespace DndWebApp.Api.Services.Constants;

public static class SortArmorOption
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string AC = "AC";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Category, AC, Value, Weight, Rarity
    };
}

public static class SortItemOption
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Category, Value, Weight, Rarity
    };
}

public static class SortToolOption
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Value = "Value";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Category, Value, Rarity
    };
}

public static class SortWeaponOption
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Type = "Type";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Category, Type, Value, Weight, Rarity
    };
}

public static class SortCharacterOption
{
    public const string Name = "Name";
    public const string TimeCreated = "TimeCreated";
    public const string Level = "Level";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, TimeCreated, Level
    };
}

public static class SortLanguageOption
{
    public const string Name = "Name";
    public const string Family = "Family";
    public const string Script = "Script";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Family, Script
    };
}

public static class SortSkillOption
{
    public const string Name = "Name";
    public const string Ability = "Ability";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Ability
    };
}

public static class SortSpellOption
{
    public const string Name = "Name";
    public const string Level = "Level";
    public const string CastingTime = "CastingTime";
    public const string Duration = "Duration";
    public const string Target = "Target";
    public const string Range = "Range";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Level, CastingTime, Duration, Target, Range
    };
}

public static class SortClassFeatureOption
{
    public const string Name = "Name";
    public const string Class = "Class";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Class
    };
}

public static class SortTraitOption
{
    public const string Name = "Name";
    public const string Race = "Race";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Race
    };
}

public static class SortBackgroundFeatureOption
{
    public const string Name = "Name";
    public const string Background = "Background";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Background
    };
}