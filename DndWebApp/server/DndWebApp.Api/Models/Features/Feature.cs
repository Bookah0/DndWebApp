using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World.Enums;

namespace DndWebApp.Api.Models.Characters;

public class Feature
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public AbilityType SavingThrows { get; set; }
    public ICollection<AbilityValue> AbilityIncreases { get; set; } = [];
    public ICollection<Spell> SpellsGained { get; set; } = [];

    // Damage affinity
    public DamageType DamageResistanceGained { get; set; }
    public DamageType DamageImmunityGained { get; set; } 
    public DamageType DamageWeaknessGained { get; set; }

    // Proficiencies
    public SkillType SkillProficiencies { get; set; }
    public WeaponCategory WeaponProficiencies { get; set; }
    public ArmorCategory ArmorProficiencies { get; set; }
    public ToolCategory ToolProficiencies { get; set; }
    public LanguageType Languages { get; set; }

    // Proficiency choices
    public ICollection<AbilityIncreaseChoice> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyChoice> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyChoice> ToolProficiencyChoices { get; set; } = [];  
    public ICollection<LanguageChoice> LanguageChoices { get; set; } = []; 
    public ICollection<ArmorProficiencyChoice> ArmorProficiencyChoices { get; set; } = [];  
    public ICollection<WeaponProficiencyChoice> WeaponProficiencyChoices { get; set; } = [];  
}

public abstract class Choice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public required int NumberOfChoices { get; set; }
}

public class SkillProficiencyChoice : Choice
{
    public required SkillType Options { get; set; }
}

public class ItemChoice : Choice
{
    public required ICollection<Item> Options { get; set; }
}

public class AbilityIncreaseChoice : Choice
{
    public required ICollection<AbilityValue> Options { get; set; }
}

public class ToolProficiencyChoice : Choice
{
    public required ToolCategory Options { get; set; }
}

public class LanguageChoice : Choice
{
    public required LanguageType Options { get; set; }
}

public class WeaponProficiencyChoice : Choice
{
    public required WeaponCategory Options { get; set; }
}

public class ArmorProficiencyChoice : Choice
{
    public required ArmorCategory Options { get; set; }
}