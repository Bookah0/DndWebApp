using DndWebApp.Api.Models.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;


public class ClassLevel
{
    public int Id { get; set; }
    public required int Level { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<ClassFeature> NewFeatures { get; set; } = [];

    public int CantripsKnown { get; set; }
    public int SpellsKnown { get; set; }
    public int[]? SpellSlots { get; set; }
    public ICollection<ClassSpecificSlot> ClassSpecificSlotsAtLevel { get; set; } = [];
    public required AClass Class { get; set; }
    public required int ClassId { get; set; }
}

[Owned]
public class ClassSpecificSlot
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}
