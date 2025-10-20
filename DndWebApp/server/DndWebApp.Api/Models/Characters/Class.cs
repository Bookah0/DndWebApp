using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

// Classes based on https://www.dnd5eapi.co/api/2014/classes/
// Subclasses based on https://www.dnd5eapi.co/api/2014/subclasses/
// Class levels based on https://www.dnd5eapi.co/api/2014/classes/{class}/levels and https://www.dnd5eapi.co/api/2014/subclasses/{subclass}/levels 
public class Class
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HitDie { get; set; }
    public Spellcasting? Spellcasting { get; set; }
    public required ICollection<ClassLevel> ClassLevels { get; set; }
    public ICollection<Item> StartingEquipment { get; set; } = [];
    public ICollection<Item> StartingEquipmentOptions { get; set; } = [];
}

[Owned]
public class ClassLevel
{
    public required int Level { get; set; }
    public required int AbilityScoreBonus { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<ClassFeature> NewFeatures { get; set; } = [];
    public SpellSlotsAtLevel? SpellSlotsAtLevel { get; set; }
    public ICollection<ClassSpecificSlot> ClassSpecificSlotsAtLevel { get; set; } = [];
}

[Owned]
public class ClassSpecificSlot
{
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}