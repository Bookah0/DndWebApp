using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

public class CharacterFeature
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;

    public ICollection<AbilityValue> AbilityIncreases { get; set; } = [];
    public ICollection<Spell> SpellsGained { get; set; } = [];

    // Damage affinity
    public ICollection<DamageType> DamageResistanceGained { get; set; } = [];
    public ICollection<DamageType> DamageImmunityGained { get; set; } = [];
    public ICollection<DamageType> DamageWeaknessGained { get; set; } = [];

    // Proficiencies
    public ICollection<Ability> SavingThrows { get; set; } = [];
    public ICollection<Skill> SkillProficiencies { get; set; } = [];
    public ICollection<WeaponCategory> WeaponProficiencies { get; set; } = [];
    public ICollection<ArmorCategory> ArmorProficiencies { get; set; } = [];
    public ICollection<Tool> ToolProficiencies { get; set; } = [];
    public ICollection<Language> Languages { get; set; } = [];

    // Proficiency choices
    public ICollection<AbilityIncreaseChoice> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyChoice> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyChoice> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageChoice> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyChoice> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponProficiencyChoice> WeaponProficiencyChoices { get; set; } = [];
}