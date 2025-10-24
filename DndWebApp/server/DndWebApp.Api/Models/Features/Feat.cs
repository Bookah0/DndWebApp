
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Models.Characters;

// Based on https://api.open5e.com/v2/feats/
public class Feat : Feature
{
    public string Prerequisite { get; set; } = "";
    public int? FromClassId { get; set; }
    public Class? FromClass { get; set; }

    public int? FromRaceId { get; set; }
    public Race? FromRace { get; set; }

    public int? FromBackgroundId { get; set; }
    public Background? FromBackground { get; set; }
}

public enum FeatFromType { Class, Background, Race }