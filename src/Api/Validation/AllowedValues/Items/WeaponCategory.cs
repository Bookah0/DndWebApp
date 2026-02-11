namespace Api.Validation.AllowedValues.Items;

public class WeaponCategory : IAllowedValuesProvider
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

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}