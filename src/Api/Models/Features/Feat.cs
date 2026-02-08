
using Api.Models.Characters;

namespace Api.Models.Features;

// Based on https://api.open5e.com/v2/feats/
public class Feat : Feature
{
    public string Prerequisite { get; set; } = "";
    public int? FromClassId { get; set; }
    public BaseClass? FromClass { get; set; }

    public int? FromRaceId { get; set; }
    public Race? FromRace { get; set; }

    public int? FromBackgroundId { get; set; }
    public Background? FromBackground { get; set; }
}
