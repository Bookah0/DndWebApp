namespace Api.Domain.Shared.Enums.Items;

public class ItemRarity : IValuesProvider
{
    public const string Common = "Common";
    public const string Uncommon = "Uncommon";
    public const string Rare = "Rare";
    public const string VeryRare = "VeryRare";
    public const string Legendary = "Legendary";
    public const string Artifact = "Artifact";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Common,
        Uncommon,
        Rare,
        VeryRare,
        Legendary,
        Artifact
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}