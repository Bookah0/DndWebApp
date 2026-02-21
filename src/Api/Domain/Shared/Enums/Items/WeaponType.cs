namespace Api.Domain.Shared.Enums.Items;

public class WeaponType : IValuesProvider
{
    public const string Club = "Club";
    public const string Dagger = "Dagger";
    public const string Greatclub = "Greatclub";
    public const string Handaxe = "Handaxe";
    public const string Javelin = "Javelin";
    public const string LightHammer = "Light Hammer";
    public const string Mace = "Mace";
    public const string Quarterstaff = "Quarterstaff";
    public const string Sickle = "Sickle";
    public const string Spear = "Spear";
    public const string LightCrossbow = "Light Crossbow";
    public const string Dart = "Dart";
    public const string Shortbow = "Shortbow";
    public const string Sling = "Sling";
    public const string Battleaxe = "Battleaxe";
    public const string Flail = "Flail";
    public const string Glaive = "Glaive";
    public const string Greataxe = "Greataxe";
    public const string Greatsword = "Greatsword";
    public const string Halberd = "Halberd";
    public const string Lance = "Lance";
    public const string Longsword = "Longsword";
    public const string Maul = "Maul";
    public const string Morningstar = "Morningstar";
    public const string Pike = "Pike";
    public const string Rapier = "Rapier";
    public const string Scimitar = "Scimitar";
    public const string Shortsword = "Shortsword";
    public const string Trident = "Trident";
    public const string WarPick = "War Pick";
    public const string Warhammer = "Warhammer";
    public const string Whip = "Whip";
    public const string Blowgun = "Blowgun";
    public const string HandCrossbow = "Hand Crossbow";
    public const string HeavyCrossbow = "Heavy Crossbow";
    public const string Longbow = "Longbow";
    public const string Net = "Net";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        Club,
        Dagger,
        Greatclub,
        Handaxe,
        Javelin,
        LightHammer,
        Mace,
        Quarterstaff,
        Sickle,
        Spear,
        LightCrossbow,
        Dart,
        Shortbow,
        Sling,
        Battleaxe,
        Flail,
        Glaive,
        Greataxe,
        Greatsword,
        Halberd,
        Lance,
        Longsword,
        Maul,
        Morningstar,
        Pike,
        Rapier,
        Scimitar,
        Shortsword,
        Trident,
        WarPick,
        Warhammer,
        Whip,
        Blowgun,
        HandCrossbow,
        HeavyCrossbow,
        Longbow,
        Net
    };  

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}