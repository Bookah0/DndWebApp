using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World.Enums;

namespace DndWebApp.Api.Models.Features;

public abstract class AFeature
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public List<AbilityType> SavingThrows { get; set; } = [];
    public ICollection<AbilityValue> AbilityIncreases { get; set; } = [];
    public ICollection<Spell> SpellsGained { get; set; } = [];

    // Damage affinity
    public DamageType DamageResistanceGained { get; set; }
    public DamageType DamageImmunityGained { get; set; } 
    public DamageType DamageWeaknessGained { get; set; }

    // Proficiencies
    public List<SkillType> SkillProficiencies { get; set; } = [];
    public List<WeaponCategory> WeaponProficiencies { get; set; } = [];
    public List<ArmorCategory> ArmorProficiencies { get; set; } = [];
    public List<ToolCategory> ToolProficiencies { get; set; } = [];
    public List<LanguageType> Languages { get; set; } = [];

    // Proficiency choices
    public ICollection<AbilityIncreaseChoice> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyChoice> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyChoice> ToolProficiencyChoices { get; set; } = [];  
    public ICollection<LanguageChoice> LanguageChoices { get; set; } = []; 
    public ICollection<ArmorProficiencyChoice> ArmorProficiencyChoices { get; set; } = [];  
    public ICollection<WeaponProficiencyChoice> WeaponProficiencyChoices { get; set; } = [];  
}