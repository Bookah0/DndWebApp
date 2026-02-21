using Api.Domain.Abilities.Models;
using Api.Domain.Languages.Models;
using Api.Domain.Skills.Models;
using Api.Domain.Spells.Models;

namespace Api.Domain.Shared.Models;

public abstract class Feature : CreatableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ICollection<AbilityValue> AbilityIncreases { get; set; } = [];
    public ICollection<Spell> SpellsGained { get; set; } = [];

    // Damage Affinities
    public ICollection<string> DamageResistanceGained { get; set; } = [];
    public ICollection<string> DamageImmunityGained { get; set; } = [];
    public ICollection<string> DamageWeaknessGained { get; set; } = [];

    // Proficiencies
    public ICollection<Ability> SavingThrowProficiencies { get; set; } = [];
    public ICollection<Skill> SkillProficiencies { get; set; } = [];
    public ICollection<Language> Languages { get; set; } = [];
    public ICollection<string> ToolProficiencies { get; set; } = [];
    public ICollection<string> WeaponCategoryProficiencies { get; set; } = [];
    public ICollection<string> WeaponTypeProficiencies { get; set; } = [];
    public ICollection<string> ArmorProficiencies { get; set; } = [];

    // Proficiency Choices
    public ICollection<AbilityIncreaseChoice> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyChoice> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyChoice> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageChoice> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyChoice> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponCategoryProficiencyChoice> WeaponCategoryProficiencyChoices { get; set; } = [];
    public ICollection<WeaponTypeProficiencyChoice> WeaponTypeProficiencyChoices { get; set; } = [];
}