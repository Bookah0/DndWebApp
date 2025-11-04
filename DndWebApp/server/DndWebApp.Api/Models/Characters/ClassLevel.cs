using DndWebApp.Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;


public class ClassLevel
{
    public int Id { get; set; }
    public required int Level { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<ClassFeature> NewFeatures { get; set; } = [];
    public SpellSlotsAtLevel? SpellSlotsAtLevel { get; set; }
    public ICollection<ClassSpecificSlot> ClassSpecificSlotsAtLevel { get; set; } = [];
    public required Class Class { get; set; }
    public required int ClassId { get; set; }
}

[Owned]
public class SpellSlotsAtLevel
{
    public int CantripsKnown { get; set; }
    public int SpellsKnown { get; set; }
    public int Lvl1 { get; set; }
    public int Lvl2 { get; set; }
    public int Lvl3 { get; set; }
    public int Lvl4 { get; set; }
    public int Lvl5 { get; set; }
    public int Lvl6 { get; set; }
    public int Lvl7 { get; set; }
    public int Lvl8 { get; set; }
    public int Lvl9 { get; set; }
}

[Owned]
public class ClassSpecificSlot
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}
