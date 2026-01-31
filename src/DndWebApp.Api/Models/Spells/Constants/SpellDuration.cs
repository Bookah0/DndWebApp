namespace DndWebApp.Api.Models.Spells.Constants;

public static class SpellDuration
{
    public const string Instantaneous = "Instantaneous";
    public const string Minute = "Minute";
    public const string Hour = "Hour";
    public const string UntilDispelled = "Until Dispelled";
    public const string Special = "Special";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Instantaneous,
        Minute,
        Hour,
        UntilDispelled,
        Special
    };
}