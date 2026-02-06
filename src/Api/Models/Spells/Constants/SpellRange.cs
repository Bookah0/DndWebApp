namespace Api.Models.Spells.Constants;

public static class SpellRange
{
    public const string Self = "Self";
    public const string Touch = "Touch";
    public const string Feet = "Feet";
    public const string Mile = "Mile";
    public const string Unlimited = "Unlimited";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Self,
        Touch,
        Feet,
        Mile,
        Unlimited
    };
}