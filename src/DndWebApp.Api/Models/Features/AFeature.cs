using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Features;

public abstract class AFeature
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public ICollection<AbilityValue> AbilityIncreases { get; set; } = [];
    public ICollection<Spell> SpellsGained { get; set; } = [];

    // Damage Affinities
    public ICollection<DamageType> DamageResistanceGained { get; set; } = [];
    public ICollection<DamageType> DamageImmunityGained { get; set; } = [];
    public ICollection<DamageType> DamageWeaknessGained { get; set; } = [];

    // Proficiencies
    public ICollection<AbilityType> SavingThrowProficiencies { get; set; } = [];
    public ICollection<SkillType> SkillProficiencies { get; set; } = [];
    public ICollection<WeaponCategory> WeaponCategoryProficiencies { get; set; } = [];
    public ICollection<WeaponType> WeaponTypeProficiencies { get; set; } = [];
    public ICollection<ArmorCategory> ArmorProficiencies { get; set; } = [];
    public ICollection<ToolCategory> ToolProficiencies { get; set; } = [];
    public ICollection<LanguageType> Languages { get; set; } = [];

    // Proficiency Choices
    public ICollection<AbilityIncreaseChoice> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyChoice> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyChoice> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageChoice> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyChoice> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponCategoryProficiencyChoice> WeaponCategoryProficiencyChoices { get; set; } = [];
    public ICollection<WeaponTypeProficiencyChoice> WeaponTypeProficiencyChoices { get; set; } = [];
}

[Owned]
public class AbilityIncreaseChoice
{
    public required string Description { get; set; }
    public required ICollection<AbilityValue> Options { get; set; }
}

[Owned]
public class SkillProficiencyChoice
{
    public required string Description { get; set; }
    public required ICollection<SkillType> Options { get; set; }
}

[Owned]
public class ToolProficiencyChoice
{
    public required string Description { get; set; }
    public required ICollection<ToolCategory> Options { get; set; }
}

[Owned]
public class LanguageChoice
{
    public required string Description { get; set; }
    public required ICollection<LanguageType> Options { get; set; }
}

[Owned]
public class WeaponCategoryProficiencyChoice
{
    public required string Description { get; set; }
    public required ICollection<WeaponCategory> Options { get; set; }
}

[Owned]
public class WeaponTypeProficiencyChoice
{
    public required string Description { get; set; }
    public required ICollection<WeaponType> Options { get; set; }
}

[Owned]
public class ArmorProficiencyChoice
{
    public required string Description { get; set; }
    public required ICollection<ArmorCategory> Options { get; set; }
}