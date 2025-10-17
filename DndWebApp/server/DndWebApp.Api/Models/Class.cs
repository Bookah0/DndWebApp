namespace DndWebApp.Api.Models;

public class Class
{
    public class DnDClass
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required Dice HitDie { get; set; }
        public required int CurrentClassLevel { get; set; }
        public Spellcasting? Spellcasting { get; set; }
        public required List<ClassLevel> ClassLevels { get; set; }
        public List<StartingItem>? StartingEquipment { get; set; }
        public List<ChoiceOption<StartingItem>>? StartingEquipmentOptions { get; set; }
        public List<ChoiceOption<Skill>>? SkillProficiencyChoices { get; set; }
        public List<Ability>? SavingThrows { get; set; }
    }

    // --- Supporting Models ---
    public class Spellcasting
    {
        public required int Level { get; set; }
        public List<SpellcastingInfo>? Info { get; set; }
        public required Ability SpellcastingAbility { get; set; }
    }

    public class SpellcastingInfo
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
    }

    public class StartingItem
    {
        public required int Quantity { get; set; }
        public required Item Item { get; set; }
    }

    public class ChoiceOption<T>
    {
        public required string Description { get; set; }
        public required int NumberOfChoices { get; set; }
        public required List<T> Choices { get; set; }
    }

    public class ClassLevel
    {
        public required int Level { get; set; }
        public required int AbilityScoreBonus { get; set; }
        public required int ProficiencyBonus { get; set; }
        public List<Feature>? NewFeatures { get; set; }
        public SpellSlots? SpellSlotsAtLevel { get; set; }
        public List<ClassSpecificSlot>? ClassSpecificSlotsAtLevel { get; set; }
    }

    public class SpellSlots
    {
        public required int ClassLevel { get; set; }
        public required int CantripsKnown { get; set; }
        public required int SpellsKnown { get; set; }
        public required int Lvl1 { get; set; }
        public required int Lvl2 { get; set; }
        public required int Lvl3 { get; set; }
        public required int Lvl4 { get; set; }
        public required int Lvl5 { get; set; }
        public required int Lvl6 { get; set; }
        public required int Lvl7 { get; set; }
        public required int Lvl8 { get; set; }
        public required int Lvl9 { get; set; }
    }
    
    public class ClassSpecificSlot
    {
        public required string Name { get; set; }
        public required int Quantity { get; set; }
    }
}