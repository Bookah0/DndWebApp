using Api.Domain.Shared.Models;

namespace Api.Domain.Classes.Models;

// Based on https://www.dnd5eapi.co/api/2014/features/
public class ClassFeature : Feature
{
    public ClassLevel? Level { get; set; }
    public required int LevelId { get; set; }
    public required int ClassId { get; set; }
}
