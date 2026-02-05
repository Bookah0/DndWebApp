namespace Api.Models.Spells.Constants;

public static class MagicSchool
{
    public const string Abjuration = "Abjuration";
    public const string Conjuration = "Conjuration";
    public const string Divination = "Divination";
    public const string Enchantment = "Enchantment";
    public const string Evocation = "Evocation";
    public const string Illusion = "Illusion";
    public const string Necromancy = "Necromancy";
    public const string Transmutation = "Transmutation";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Abjuration,
        Conjuration,
        Divination,
        Enchantment,
        Evocation,
        Illusion,
        Necromancy,
        Transmutation
    };
}