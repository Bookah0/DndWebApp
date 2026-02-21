namespace Api.Domain.Shared.Enums.Items;

public class ToolCategory : IValuesProvider
{
    public const string AlchemistsSupplies = "Alchemist's Supplies";
    public const string BrewersSupplies = "Brewer's Supplies";
    public const string CalligraphersSupplies = "Calligrapher's Supplies";
    public const string CarpentersTools = "Carpenter's Tools";
    public const string CartographersTools = "Cartographer's Tools";
    public const string CobblersTools = "Cobbler's Tools";
    public const string CooksUtensils = "Cook's Utensils";
    public const string DiceSet = "Dice Set";
    public const string DisguiseKit = "Disguise Kit";
    public const string DragonChessSet = "Dragon Chess Set";
    public const string ForgeryKit = "Forgery Kit";
    public const string GlassblowersTools = "Glassblower's Tools";
    public const string HerbalismKit = "Herbalism Kit";
    public const string JewelersTools = "Jeweler's Tools";
    public const string LandVehicles = "Land Vehicles";
    public const string LeatherworkersTools = "Leatherworker's Tools";
    public const string MasonsTools = "Mason's Tools";
    public const string NavigatorsTools = "Navigator's Tools";
    public const string Net = "Net";
    public const string PaintersSupplies = "Painter's Supplies";
    public const string PlayingCardSet = "Playing Card Set";
    public const string PoisonersKit = "Poisoner's Kit";
    public const string PottersTools = "Potter's Tools";
    public const string SmithsTools = "Smith's Tools";
    public const string ThievesTools = "Thieves' Tools";
    public const string ThreeDragonAnteSet = "Three Dragon Ante Set";
    public const string TinkersTools = "Tinker's Tools";
    public const string WaterVehicles = "Water Vehicles";
    public const string WeaversTools = "Weaver's Tools";
    public const string WoodcarversTools = "Woodcarver's Tools";
    public const string GamingSet = "Gaming Set";
    public const string MusicalInstrument = "Musical Instrument";
    public const string ArtisansTools = "Artisan's Tools";
    public const string DragonscaleSmithingTools = "Dragonscale Smithing Tools";
    public const string DungeoneersPack = "Dungeoneer's Pack";
    public const string FletchingTools = "Fletching Tools";

    public static readonly IReadOnlySet<string> Values = new HashSet<string>
    {
        AlchemistsSupplies,
        BrewersSupplies,
        CalligraphersSupplies,
        CarpentersTools,
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
        LeatherworkersTools,
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
        WoodcarversTools,
        GamingSet,
        MusicalInstrument,
        ArtisansTools,
        DragonscaleSmithingTools,
        DungeoneersPack,
        FletchingTools
    };

    static IReadOnlySet<string> IValuesProvider.Values => Values;
}