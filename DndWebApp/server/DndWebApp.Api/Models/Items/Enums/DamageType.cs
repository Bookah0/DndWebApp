namespace DndWebApp.Api.Models.Items.Enums;

[Flags]
public enum AffinityType
{
    Immune,
    Resistant,
    Weakness
}

[Flags]
public enum DamageType
{
    None = 0,
    Acid = 1 << 0,
    Bludgeoning = 1 << 1,
    Cold = 1 << 2,
    Fire = 1 << 3,
    Force = 1 << 4,
    Lightning = 1 << 5,
    Necrotic = 1 << 6,
    Piercing = 1 << 7,
    Poison = 1 << 8,
    Psychic = 1 << 9,
    Radiant = 1 << 10,
    Slashing = 1 << 11,
    Thunder = 1 << 12
}
