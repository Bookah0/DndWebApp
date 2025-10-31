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
    public ICollection<AbilityType> SavingThrows { get; set; } = [];
    public ICollection<AbilityValue> AbilityIncreases { get; set; } = [];
    public ICollection<Spell> SpellsGained { get; set; } = [];

    // Damage affinity
    public ICollection<DamageType> DamageResistanceGained { get; set; } = [];
    public ICollection<DamageType> DamageImmunityGained { get; set; } = [];
    public ICollection<DamageType> DamageWeaknessGained { get; set; } = [];

    // Proficiencies
    public List<SkillType> SkillProficiencies { get; set; } = [];
    public List<WeaponCategory> WeaponProficiencies { get; set; } = [];
    public List<ArmorCategory> ArmorProficiencies { get; set; } = [];
    public List<ToolCategory> ToolProficiencies { get; set; } = [];
    public List<LanguageType> Languages { get; set; } = [];

    // Proficiency choices
    public ICollection<AbilityIncreaseOption> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyOption> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyOption> ToolProficiencyChoices { get; set; } = [];  
    public ICollection<LanguageOption> LanguageChoices { get; set; } = []; 
    public ICollection<ArmorProficiencyOption> ArmorProficiencyChoices { get; set; } = [];  
    public ICollection<WeaponProficiencyOption> WeaponProficiencyChoices { get; set; } = [];  
}