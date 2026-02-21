namespace Api.Domain.Shared.Enums;

// Might not be needed after db layer sorting is implemented
public class SortArmorOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string AC = "AC";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, AC, Value, Weight, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortItemOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, Value, Weight, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortToolOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Value = "Value";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, Value, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortWeaponOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Type = "Type";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, Type, Value, Weight, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortCharacterOption : IValuesProvider
{
    public const string Name = "Name";
    public const string TimeCreated = "TimeCreated";
    public const string Level = "Level";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, TimeCreated, Level
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortLanguageOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Family = "Family";
    public const string Script = "Script";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Family, Script
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortSkillOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Ability = "Ability";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Ability
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortSpellOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Level = "Level";
    public const string CastingTime = "CastingTime";
    public const string Duration = "Duration";
    public const string Target = "Target";
    public const string Range = "Range";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Level, CastingTime, Duration, Target, Range
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortClassFeatureOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Class = "Class";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Class
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortTraitOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Race = "Race";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Race
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}

public class SortBackgroundFeatureOption : IValuesProvider
{
    public const string Name = "Name";
    public const string Background = "Background";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Background
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}