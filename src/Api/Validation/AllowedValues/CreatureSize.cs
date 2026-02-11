namespace Api.Validation.AllowedValues;

public class CreatureSize : IAllowedValuesProvider
{
    public const string Tiny = "Tiny";
    public const string Small = "Small";
    public const string Medium = "Medium";
    public const string Large = "Large";
    public const string Huge = "Huge";
    public const string Gargantuan = "Gargantuan";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Tiny,
        Small,
        Medium,
        Large,
        Huge,
        Gargantuan
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}