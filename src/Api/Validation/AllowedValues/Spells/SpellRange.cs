namespace Api.Validation.AllowedValues.Spells;

public class SpellRange : IAllowedValuesProvider
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

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}