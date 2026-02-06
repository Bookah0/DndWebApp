namespace Api.Models.Items.Constants;

public static class ArmorCategory
{
    public const string Light = "Light";
    public const string Medium = "Medium";
    public const string Heavy = "Heavy";
    public const string Shield = "Shield";
    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        Light,
        Medium,
        Heavy,
        Shield
    };
}