namespace Api.Domain.Shared.Enums.Character;

public class AbilityType : IValuesProvider
{
    public const string Strength = "Strength";
    public const string Dexterity = "Dexterity";
    public const string Constitution = "Constitution";
    public const string Intelligence = "Intelligence";
    public const string Wisdom = "Wisdom";
    public const string Charisma = "Charisma";
    
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}