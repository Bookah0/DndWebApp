

namespace DndWebApp.Api.Models.Items.Constants;

public static class DamageType
{
    public const string Acid = "Acid";
    public const string Bludgeoning = "Bludgeoning";
    public const string Cold = "Cold";
    public const string Fire = "Fire";
    public const string Force = "Force";
    public const string Lightning = "Lightning";
    public const string Necrotic = "Necrotic";
    public const string Piercing = "Piercing";
    public const string Poison = "Poison";
    public const string Psychic = "Psychic";
    public const string Radiant = "Radiant";
    public const string Slashing = "Slashing";
    public const string Thunder = "Thunder";
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Acid,
        Bludgeoning,
        Cold,
        Fire,
        Force,
        Lightning,
        Necrotic,
        Piercing,
        Poison,
        Psychic,
        Radiant,
        Slashing,
        Thunder
    };
}