namespace Api.Domain.Shared.Enums.Character;

public class AlignmentType : IValuesProvider
{
    public const string LawfulGood = "Lawful Good";
    public const string NeutralGood = "Neutral Good";
    public const string ChaoticGood = "Chaotic Good";
    public const string LawfulNeutral = "Lawful Neutral";
    public const string TrueNeutral = "True Neutral";
    public const string ChaoticNeutral = "Chaotic Neutral";
    public const string LawfulEvil = "Lawful Evil";
    public const string NeutralEvil = "Neutral Evil";
    public const string ChaoticEvil = "Chaotic Evil";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        LawfulGood,
        NeutralGood,
        ChaoticGood,
        LawfulNeutral,
        TrueNeutral,
        ChaoticNeutral,
        LawfulEvil,
        NeutralEvil,
        ChaoticEvil
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}