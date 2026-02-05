namespace DndWebApp.Api.Models.Items.Constants;

public static class ItemCategory
{
    public const string None = "None";
    public const string AdventuringGear = "AdventuringGear";
    public const string Ammunition = "Ammunition";
    public const string Armor = "Armor";
    public const string Art = "Art";
    public const string EquipmentPack = "EquipmentPack";
    public const string Gem = "Gem";
    public const string Jewelry = "Jewelry";
    public const string Instrument = "Instrument";
    public const string Vehicle = "Vehicle";
    public const string Mount = "Mount";
    public const string Poison = "Poison";
    public const string Potion = "Potion";
    public const string Ring = "Ring";
    public const string Rod = "Rod";
    public const string Scroll = "Scroll";
    public const string Service = "Service";
    public const string Shield = "Shield";
    public const string SpellcastingFocus = "SpellcastingFocus";
    public const string Staff = "Staff";
    public const string Tools = "Tools";
    public const string TradeGood = "TradeGood";
    public const string Wand = "Wand";
    public const string Weapon = "Weapon";
    public const string WondrousItem = "WondrousItem";
    public const string Utility = "Utility";
    public const string Miscellaneous = "Miscellaneous";

    public static readonly IReadOnlySet<string> AllowedValues = new HashSet<string>
    {
        None,
        AdventuringGear,
        Ammunition,
        Armor,
        Art,
        EquipmentPack,
        Gem,
        Jewelry,
        Instrument,
        Vehicle,
        Mount,
        Poison,
        Potion,
        Ring,
        Rod,
        Scroll,
        Service,
        Shield,
        SpellcastingFocus,
        Staff,
        Tools,
        TradeGood,
        Wand,
        Weapon,
        WondrousItem,
        Utility,
        Miscellaneous
    };
}









