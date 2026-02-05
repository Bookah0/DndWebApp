namespace Api.Models.Items.Constants;

public static class WeaponCategory
{
    public const string SimpleMelee = "Simple Melee";
    public const string SimpleRanged = "Simple Ranged";
    public const string MartialMelee = "Martial Melee";
    public const string MartialRanged = "Martial Ranged";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        SimpleMelee,
        SimpleRanged,
        MartialMelee,
        MartialRanged
    };
}