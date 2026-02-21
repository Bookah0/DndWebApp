namespace Api.Domain.Shared.Enums.Items;

public class WeaponCategory : IValuesProvider
{
    public const string SimpleMelee = "Simple Melee";
    public const string SimpleRanged = "Simple Ranged";
    public const string MartialMelee = "Martial Melee";
    public const string MartialRanged = "Martial Ranged";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        SimpleMelee,
        SimpleRanged,
        MartialMelee,
        MartialRanged
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}