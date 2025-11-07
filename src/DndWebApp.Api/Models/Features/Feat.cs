
using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.Features;

// Based on https://api.open5e.com/v2/feats/
public class Feat : AFeature
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