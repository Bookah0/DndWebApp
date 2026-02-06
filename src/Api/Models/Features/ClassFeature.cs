using Api.Models.Characters;
using Api.Services.Util.Interfaces;

namespace Api.Models.Features;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class ClassFeature : Feature
{
    public ClassLevel? Level { get; set; }
    public required int LevelId { get; set; }
    public required int ClassId { get; set; }
}
