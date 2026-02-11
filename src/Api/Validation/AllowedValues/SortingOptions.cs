namespace Api.Validation.AllowedValues;

// Might not be needed after db layer sorting is implemented
public class SortArmorOption : IAllowedValuesProvider
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

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortItemOption : IAllowedValuesProvider
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

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortToolOption : IAllowedValuesProvider
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Value = "Value";
    public const string Rarity = "Rarity";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Category, Value, Rarity
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortWeaponOption : IAllowedValuesProvider
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

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortCharacterOption : IAllowedValuesProvider
{
    public const string Name = "Name";
    public const string TimeCreated = "TimeCreated";
    public const string Level = "Level";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, TimeCreated, Level
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortLanguageOption : IAllowedValuesProvider
{
    public const string Name = "Name";
    public const string Family = "Family";
    public const string Script = "Script";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Family, Script
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortSkillOption : IAllowedValuesProvider
{
    public const string Name = "Name";
    public const string Ability = "Ability";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Ability
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortSpellOption : IAllowedValuesProvider
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

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortClassFeatureOption : IAllowedValuesProvider
{
    public const string Name = "Name";
    public const string Class = "Class";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Class
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortTraitOption : IAllowedValuesProvider
{
    public const string Name = "Name";
    public const string Race = "Race";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Race
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}

public class SortBackgroundFeatureOption : IAllowedValuesProvider
{
    public const string Name = "Name";
    public const string Background = "Background";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Name, Background
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}