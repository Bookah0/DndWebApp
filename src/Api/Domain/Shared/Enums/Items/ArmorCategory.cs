namespace Api.Domain.Shared.Enums.Items;

public class ArmorCategory : IValuesProvider
{
    public const string Light = "Light";
    public const string Medium = "Medium";
    public const string Heavy = "Heavy";
    public const string Shield = "Shield";
    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Light,
        Medium,
        Heavy,
        Shield
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}