using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Models.Spells;

public class Spellcasting
{
    public int Id { get; set; }
    public required int Level { get; set; }
    public List<SpellcastingInfo> Info { get; set; } = [];
    public required AbilityValue SpellcastingAbility { get; set; }
}

public class SpellcastingInfo
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}

public class SpellSlotsAtLevel
{
    public int Id { get; set; }
    public required Class Parent { get; set; }
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

public class CurrentSpellSlots
{
    public int Id { get; set; }
    public required Character Parent { get; set; }
    public required int Lvl1 { get; set; }
    public int Lvl2 { get; set; } = 0;
    public int Lvl3 { get; set; } = 0;
    public int Lvl4 { get; set; } = 0;
    public int Lvl5 { get; set; } = 0;
    public int Lvl6 { get; set; } = 0;
    public int Lvl7 { get; set; } = 0;
    public int Lvl8 { get; set; } = 0;
    public int Lvl9 { get; set; } = 0;
}