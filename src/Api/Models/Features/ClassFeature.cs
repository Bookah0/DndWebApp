using Api.Models.Characters;
using Api.Services.Util.Interfaces;

namespace Api.Models.Features;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class ClassFeature : AFeature, IOwnedByEntity
{
    public ClassLevel? Level { get; set; }
    public required int LevelId { get; set; }
    public required int ClassId { get; set; }
    public int OwnerId => ClassId;
}
