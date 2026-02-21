namespace Api.Domain.Shared.Enums.Spells;

public class SpellRange : IValuesProvider
{
    public const string Self = "Self";
    public const string Touch = "Touch";
    public const string Feet = "Feet";
    public const string Mile = "Mile";
    public const string Unlimited = "Unlimited";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Self,
        Touch,
        Feet,
        Mile,
        Unlimited
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}