namespace Api.Models.Items.Constants;

public static class WeaponProperty
{
    public const string Ammunition = "Ammunition";
    public const string Finesse = "Finesse";
    public const string Heavy = "Heavy";
    public const string Light = "Light";
    public const string Loading = "Loading";
    public const string Range = "Range";
    public const string Reach = "Reach";
    public const string Special = "Special";
    public const string Thrown = "Thrown";
    public const string TwoHanded = "Two Handed";
    public const string Versatile = "Versatile";
    public const string Improvised = "Improvised";
    public const string Silvered = "Silvered";
    public const string Rod = "Rod";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Ammunition,
        Finesse,
        Heavy,
        Light,
        Loading,
        Range,
        Reach,
        Special,
        Thrown,
        TwoHanded,
        Versatile,
        Improvised,
        Silvered,
        Rod
    };
}