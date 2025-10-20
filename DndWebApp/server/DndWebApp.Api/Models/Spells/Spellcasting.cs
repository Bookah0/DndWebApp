using DndWebApp.Api.Models.Characters;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Spells;

[Owned]
public class Spellcasting
{
    public required int SpellLevel { get; set; }
    public required ICollection<SpellcastingInfo> Info { get; set; }
    public required int SpellcastingAbilityId { get; set; }
}

[Owned]
public class SpellcastingInfo
{
    public required string Title { get; set; }
    public required string Description { get; set; }
}

[Owned]
public class SpellSlotsAtLevel
{
    public required int ClassId { get; set; }
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

[Owned]
public class CurrentSpellSlots
{
    public required int CharacterId { get; set; }
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