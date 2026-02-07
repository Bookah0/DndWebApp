using Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.Characters;

public class ClassLevel : CreatableEntity
{
    public required int Level { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<ClassFeature> NewFeatures { get; set; } = [];

    public int CantripsKnown { get; set; }
    public int SpellsKnown { get; set; }
    public int[]? SpellSlots { get; set; }
    public ICollection<ClassSpecificSlot> ClassSpecificSlotsAtLevel { get; set; } = [];
    public required Class Class { get; set; }
    public required int ClassId { get; set; }
}

[Owned]
public class ClassSpecificSlot
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}
