namespace Api.Domain.Shared.Enums.Spells;

public class SpellType : IValuesProvider
{
    public const string None = "None";
    public const string Normal = "Normal";
    public const string Ritual = "Ritual";
    public const string Reaction = "Reaction";
    public const string Concentration = "Concentration";
    public const string AttackRoll = "AttackRoll";
    public const string SavingThrow = "SavingThrow";
    public const string Buff = "Buff";
    public const string Debuff = "Debuff";
    public const string Healing = "Healing";
    public const string Damage = "Damage";
    public const string Summoning = "Summoning";
    public const string Control = "Control";
    public const string Utility = "Utility";
    public const string AoE = "AoE";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        None,
        Normal,
        Ritual,
        Reaction,
        Concentration,
        AttackRoll,
        SavingThrow,
        Buff,
        Debuff,
        Healing,
        Damage,
        Summoning,
        Control,
        Utility,
        AoE
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}