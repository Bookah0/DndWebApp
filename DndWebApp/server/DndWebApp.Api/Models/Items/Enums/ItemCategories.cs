namespace DndWebApp.Api.Models.Items.Enums;

[Flags]
public enum ItemCategory
{
    None = 0,
    AdventuringGear = 1 << 0,
    Ammunition = 1 << 1,
    Armor = 1 << 2,
    Art = 1 << 3,
    EquipmentPack = 1 << 4,
    Gem = 1 << 5,
    Jewelry = 1 << 6,
    Instrument = 1 << 7,
    LandVehicle = 1 << 8,
    Mount = 1 << 9,
    Poison = 1 << 10,
    Potion = 1 << 11,
    Ring = 1 << 12,
    Rod = 1 << 13,
    Scroll = 1 << 14,
    Service = 1 << 15,
    Shield = 1 << 16,
    SpellcastingFocus = 1 << 17,
    Staff = 1 << 18,
    Tools = 1 << 19,
    TradeGood = 1 << 20,
    Wand = 1 << 21,
    WaterborneVehicle = 1 << 22,
    Weapon = 1 << 23,
    WondrousItem = 1 << 24,
    Utility = 1 << 25
}

public enum ArmorCategory
{
    Light,
    Medium,
    Heavy,
    Shield
}

[Flags]
public enum WeaponCategory : long
{
    None = 0,

    SimpleMelee = 1 << 0,
    SimpleRanged = 1 << 1,
    MartialMelee = 1 << 2,
    MartialRanged = 1 << 3,

    Club = 1 << 4,
    Dagger = 1 << 5,
    Greatclub = 1 << 6,
    Handaxe = 1 << 7,
    Javelin = 1 << 8,
    LightHammer = 1 << 9,
    Mace = 1 << 10,
    Quarterstaff = 1 << 11,
    Sickle = 1 << 12,
    Spear = 1 << 13,
    LightCrossbow = 1 << 14,
    Dart = 1 << 15,
    Shortbow = 1 << 16,
    Sling = 1 << 17,
    Battleaxe = 1 << 18,
    Flail = 1 << 19,
    Glaive = 1 << 20,
    Greataxe = 1 << 21,
    Greatsword = 1 << 22,
    Halberd = 1 << 23,
    Lance = 1 << 24,
    Longsword = 1 << 25,
    Maul = 1 << 26,
    Morningstar = 1 << 27,
    Pike = 1 << 28,
    Rapier = 1 << 29,
    Scimitar = 1 << 30,
    Shortsword = 1L << 31,
    Trident = 1L << 32,
    WarPick = 1L << 33,
    Warhammer = 1L << 34,
    Whip = 1L << 35,
    Blowgun = 1L << 36,
    HandCrossbow = 1L << 37,
    HeavyCrossbow = 1L << 38,
    Longbow = 1L << 39,
    Net = 1L << 40
}

public enum ToolCategory
{
    AlchemistsSupplies,
    BrewersSupplies,
    CalligraphersSupplies,
    CarpenterTools,
    CartographersTools,
    CobblersTools,
    CooksUtensils,
    DiceSet,
    DisguiseKit,
    DragonChessSet,
    ForgeryKit,
    GlassblowersTools,
    HerbalismKit,
    JewelersTools,
    LandVehicles,
    LeatherworkerTools,
    MasonsTools,
    NavigatorsTools,
    Net,
    PaintersSupplies,
    PlayingCardSet,
    PoisonersKit,
    PottersTools,
    SmithsTools,
    ThievesTools,
    ThreeDragonAnteSet,
    TinkersTools,
    WaterVehicles,
    WeaversTools,
    WoodcarversTools
}

public enum InstrumentCategory
{
    Bagpipes,
    Drum,
    Dulcimer,
    Flute,
    Horn,
    Lute,
    Lyre,
    PanFlute,
    Shawm,
    Viol,
}