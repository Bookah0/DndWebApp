namespace Api.Domain.Shared.Enums;
public class SortArmorOption : IValuesProvider, IHasDefault 
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string AC = "AC";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, AC, Value, Weight, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortItemOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, Value, Weight, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortToolOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Value = "Value";
    public const string Rarity = "Rarity";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, Value, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortWeaponOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Category = "Category";
    public const string Type = "Type";
    public const string Value = "Value";
    public const string Weight = "Weight";
    public const string Rarity = "Rarity";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Category, Type, Value, Weight, Rarity
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortCharacterOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string TimeCreated = "TimeCreated";
    public const string Level = "Level";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, TimeCreated, Level
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortSubclassOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string ParentClass = "ParentClass";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, ParentClass
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}


public class SortLanguageOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Family = "Family";
    public const string Script = "Script";
    public const string Default = Name;
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Family, Script
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortSkillOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Ability = "Ability";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Ability
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortSpellOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Level = "Level";
    public const string CastingTime = "CastingTime";
    public const string Duration = "Duration";
    public const string Target = "Target";
    public const string Range = "Range";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Level, CastingTime, Duration, Target, Range
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortClassFeatureOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Class = "Class";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Class
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortTraitOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Race = "Race";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Race
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}

public class SortBackgroundFeatureOption : IValuesProvider, IHasDefault
{
    public const string Name = "Name";
    public const string Background = "Background";
    public const string Default = Name;
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Name, Background
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
    static string IHasDefault.Default => Default;
}