using DndWebApp.Api.Models.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.DTOs;

public class ClassLevelDto
{
    public int Id { get; set; }
    public required int Level { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<int> NewFeatureIds { get; set; } = [];
    public SpellSlotsAtLevelDto? SpellSlotsAtLevel { get; set; }
    public ICollection<ClassSpecificSlotDto> ClassSpecificSlotsAtLevel { get; set; } = [];
    public required int ClassId { get; set; }
}

public class SpellSlotsAtLevelDto
{
    public int Id { get; set; }
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

public class ClassSpecificSlotDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}
