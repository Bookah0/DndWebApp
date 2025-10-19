using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models.Characters;

// Classes based on https://www.dnd5eapi.co/api/2014/classes/
// Subclasses based on https://www.dnd5eapi.co/api/2014/subclasses/
// Class levels based on https://www.dnd5eapi.co/api/2014/classes/{class}/levels and https://www.dnd5eapi.co/api/2014/subclasses/{subclass}/levels 
public class Class
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string HitDie { get; set; }
    public required int CurrentClassLevel { get; set; }
    public Spellcasting? Spellcasting { get; set; }
    public required List<ClassLevel> ClassLevels { get; set; }
    public List<Item> StartingEquipment { get; set; } = [];
    public List<ChoiceOption<Item>> StartingEquipmentOptions { get; set; } = [];

    // Proficiencies
    public List<ChoiceOption<string>> SkillProficiencyChoices { get; set; } = [];
    public List<AbilityValue> SavingThrows { get; set; } = [];
}

public class ClassLevel
{
    public required int Level { get; set; }
    public required int AbilityScoreBonus { get; set; }
    public required int ProficiencyBonus { get; set; }
    public List<Feature> NewFeatures { get; set; } = [];
    public SpellSlotsAtLevel? SpellSlotsAtLevel { get; set; }
    public List<ClassSpecificSlot> ClassSpecificSlotsAtLevel { get; set; } = [];
}

public class ClassSpecificSlot
{
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}