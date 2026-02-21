using Api.Domain.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Classes.Models;

public class ClassLevel : CreatableEntity
{
    public required int Level { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<ClassFeature> NewFeatures { get; set; } = [];

    public int CantripsKnown { get; set; }
    public int SpellsKnown { get; set; }
    public int[]? SpellSlots { get; set; }
    public ICollection<ClassSlot> ClassSlotsAtLevel { get; set; } = [];
    public required Class Class { get; set; }
    public required int ClassId { get; set; }
}

[Owned]
public class ClassSlot
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}
