namespace Api.Domain.Shared.Enums.Spells;

public class SpellDuration : IValuesProvider
{
    public const string Instantaneous = "Instantaneous";
    public const string Minute = "Minute";
    public const string Hour = "Hour";
    public const string Day = "Day";
    public const string Round = "Round";
    public const string UntilDispelled = "Until Dispelled";
    public const string Special = "Special";
    public const string Permanent = "Permanent";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Instantaneous,
        Minute,
        Hour,
        Day,
        UntilDispelled,
        Round,
        Special,
        Permanent
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}