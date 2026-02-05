namespace Api.Models.Characters.Constants;

public static class AbilityType
{
    public const string Strength = "Strength";
    public const string Dexterity = "Dexterity";
    public const string Constitution = "Constitution";
    public const string Intelligence = "Intelligence";
    public const string Wisdom = "Wisdom";
    public const string Charisma = "Charisma";
    
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    };
}