namespace Api.Domain.Shared.Enums.Damage;

public class AffinityType : IValuesProvider
{
    public const string Immune = "Immune";
    public const string Resistant = "Resistant";
    public const string Weakness = "Weakness";
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Immune,
        Resistant,
        Weakness
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}