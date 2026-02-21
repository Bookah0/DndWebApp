namespace Api.Domain.Shared.Enums.Spells;

public class CastingTime : IValuesProvider
{
    public const string Reaction = "Reaction";
    public const string BonusAction = "Bonus Action";
    public const string Action = "Action";
    public const string Minute = "Minute";
    public const string Hour = "Hour";
    public const string Special = "Special";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Reaction,
        BonusAction,
        Action,
        Minute,
        Hour,
        Special
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}