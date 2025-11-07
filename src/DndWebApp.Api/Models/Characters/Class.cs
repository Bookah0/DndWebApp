using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

// Classes based on https://www.dnd5eapi.co/api/2014/classes/
// Subclasses based on https://www.dnd5eapi.co/api/2014/subclasses/
// Class levels based on https://www.dnd5eapi.co/api/2014/classes/{class}/levels and https://www.dnd5eapi.co/api/2014/subclasses/{subclass}/levels 
public class AClass
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HitDie { get; set; }
    public required ICollection<ClassLevel> ClassLevels { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public AbilityType? SpellcastingAbilityId { get; set; }
}

public class Class : AClass
{
    public ICollection<Class> Subclasses { get; set; } = [];
    public ICollection<Item> StartingEquipment { get; set; } = [];
    public ICollection<StartingEquipmentOption> StartingEquipmentOptions { get; set; } = [];
}

public class Subclass : AClass
{
    public Class? ParentClass { get; set; }
    public required int ParentClassId { get; set; }
}

[Owned]
public class StartingEquipmentOption
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required ICollection<int> OptionIds { get; set; }
}