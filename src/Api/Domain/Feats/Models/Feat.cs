using Api.Domain.Backgrounds.Models;
using Api.Domain.Classes.Models;
using Api.Domain.Shared.Models;
using Api.Domain.Species.Models;

namespace Api.Domain.Feats.Models;

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
