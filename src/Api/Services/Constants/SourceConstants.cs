namespace Api.Services.Constants;

public static class FeatSourceConstants
{
    public const string Background = "Background";
    public const string Class = "Class";
    public const string Subclass = "Subclass";
    public const string Race = "Race";
    public const string Subrace = "Subrace";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Background, Class, Subclass, Race, Subrace
    };
}