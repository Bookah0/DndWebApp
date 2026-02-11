namespace Api.Validation.AllowedValues.Spells;

public class CastingTime : IAllowedValuesProvider
{
    public const string Reaction = "Reaction";
    public const string BonusAction = "Bonus Action";
    public const string Action = "Action";
    public const string Minute = "Minute";
    public const string Hour = "Hour";
    public const string Special = "Special";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Reaction,
        BonusAction,
        Action,
        Minute,
        Hour,
        Special
    };

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}