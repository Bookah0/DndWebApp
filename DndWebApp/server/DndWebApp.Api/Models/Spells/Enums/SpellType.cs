namespace DndWebApp.Api.Models.Spells;

[Flags]
public enum SpellType
{
    None = 0,
    Normal = 1 << 0,
    Ritual = 1 << 1,
    Reaction = 1 << 2,
    Concentration = 1 << 3,
    AttackRoll = 1 << 4,
    SavingThrow = 1 << 5,
    Buff = 1 << 6,
    Debuff = 1 << 7,
    Healing = 1 << 8,
    Damage = 1 << 9,
    Summoning = 1 << 10,
    Control = 1 << 11,
    Utility = 1 << 12
}