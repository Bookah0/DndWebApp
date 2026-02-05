namespace DndWebApp.Api.Models.Items.Constants;

public static class AffinityType
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
}