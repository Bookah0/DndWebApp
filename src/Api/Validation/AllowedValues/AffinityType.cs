namespace Api.Validation.AllowedValues;

public class AffinityType : IAllowedValuesProvider
{
    public const string Immune = "Immune";
    public const string Resistant = "Resistant";
    public const string Weakness = "Weakness";
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Immune,
        Resistant,
        Weakness
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}