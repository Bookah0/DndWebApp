namespace DndWebApp.Api.Models.Items.Constants;

public static class EquipSlot
{
    public const string MainHand = "MainHand";
    public const string OffHand = "OffHand";
    public const string Ranged = "Ranged";
    public const string Armor = "Armor";
    public const string Head = "Head";
    public const string Waist = "Waist";
    public const string Hands = "Hands";
    public const string Feet = "Feet";
    public const string Back = "Back";
    public const string Neck = "Neck";
    public const string Rings = "Rings";
    public const string ArcaneFocus = "ArcaneFocus";
    public const string HolySymbol = "HolySymbol";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        MainHand,
        OffHand,
        Ranged,
        Armor,
        Head,
        Waist,
        Hands,
        Feet,
        Back,
        Neck,
        Rings,
        ArcaneFocus,
        HolySymbol
    };
}
