using DndWebApp.Api.Models.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.DTOs;

public class ClassLevelDto
{
    public int Id { get; set; }
    public required int Level { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<int> NewFeatureIds { get; set; } = [];
    public int CantripsKnown { get; set; }
    public int SpellsKnown { get; set; }
    public int[]? SpellSlotsAtLevel { get; set; }
    public ICollection<ClassSpecificSlotDto> ClassSpecificSlotsAtLevel { get; set; } = [];
    public required bool isSubclassLevel;
    public required int ClassId { get; set; }
}

public class ClassSpecificSlotDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}
