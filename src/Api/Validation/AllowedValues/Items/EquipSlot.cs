namespace Api.Validation.AllowedValues.Items;

public class EquipSlot : IAllowedValuesProvider
{
    public const string MainHand = "Main Hand";
    public const string OffHand = "Off Hand";
    public const string TwoHand = "Two Hand";
    public const string Ranged = "Ranged";
    public const string Armor = "Armor";
    public const string Head = "Head";
    public const string Waist = "Waist";
    public const string Hands = "Hands";
    public const string Feet = "Feet";
    public const string Back = "Back";
    public const string Neck = "Neck";
    public const string Rings = "Rings";
    public const string ArcaneFocus = "Arcane Focus";
    public const string HolySymbol = "Holy Symbol";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        MainHand,
        OffHand,
        TwoHand,
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

    static IReadOnlySet<string> IAllowedValuesProvider.AllowedValues => AllowedValues;
}
