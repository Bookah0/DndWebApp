using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.Items;
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
    public ICollection<AbilityType> SavingThrows { get; set; } = [];
    public ICollection<AbilityValue> AbilityIncreases { get; set; } = [];
    public ICollection<Spell> SpellsGained { get; set; } = [];

    // Damage affinity
    public ICollection<DamageType> DamageResistanceGained { get; set; } = [];
    public ICollection<DamageType> DamageImmunityGained { get; set; } = [];
    public ICollection<DamageType> DamageWeaknessGained { get; set; } = [];

    // Proficiencies
    public ICollection<SkillType> SkillProficiencies { get; set; } = [];
    public ICollection<WeaponCategory> WeaponProficiencies { get; set; } = [];
    public ICollection<ArmorCategory> ArmorProficiencies { get; set; } = [];
    public ICollection<ToolCategory> ToolProficiencies { get; set; } = [];
    public ICollection<LanguageType> Languages { get; set; } = [];

    // Proficiency choices
    public ICollection<AbilityIncreaseOption> AbilityIncreaseChoices { get; set; } = [];
    public ICollection<SkillProficiencyOption> SkillProficiencyChoices { get; set; } = [];
    public ICollection<ToolProficiencyOption> ToolProficiencyChoices { get; set; } = [];
    public ICollection<LanguageOption> LanguageChoices { get; set; } = [];
    public ICollection<ArmorProficiencyOption> ArmorProficiencyChoices { get; set; } = [];
    public ICollection<WeaponProficiencyOption> WeaponProficiencyChoices { get; set; } = [];
}

[Owned]
public class SkillProficiencyOption
{
    public required string Description { get; set; }
    public required ICollection<SkillType> Options { get; set; }
}

[Owned]
public class AbilityIncreaseOption
{
    public required string Description { get; set; }
    public required ICollection<AbilityValue> Options { get; set; }
}

[Owned]
public class ToolProficiencyOption
{
    public required string Description { get; set; }
    public required ICollection<ToolCategory> Options { get; set; }
}

[Owned]
public class LanguageOption
{
    public required string Description { get; set; }
    public required ICollection<LanguageType> Options { get; set; }
}

[Owned]
public class WeaponProficiencyOption
{
    public required string Description { get; set; }
    public required ICollection<WeaponCategory> CategoryOptions { get; set; }
    public required ICollection<WeaponType> TypeOptions { get; set; }
}

[Owned]
public class ArmorProficiencyOption
{
    public required string Description { get; set; }
    public required ICollection<ArmorCategory> Options { get; set; }
}