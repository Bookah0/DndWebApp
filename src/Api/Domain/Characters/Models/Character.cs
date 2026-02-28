namespace Api.Domain.Characters.Models;

using Api.Domain.Abilities.Models;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Classes.Models;
using Api.Domain.Feats.Models;
using Api.Domain.Shared.Models;
using Api.Domain.Species.Models;
using Api.Domain.Spells.Models;
using Microsoft.EntityFrameworkCore;

public class Character : CreatableEntity
{
    public required string Name { get; set; }
    public required int Level { get; set; }
    public int? Experience { get; set; }
    public string PlayerName { get; set; } = "";

    // Races & Subraces
    public required Race Race { get; set; }
    public int RaceId { get; set; }
    public Subrace? Subrace { get; set; }
    public int? SubraceId { get; set; }

    // Classes & Subclasses
    public required BaseClass Class { get; set; }
    public required int ClassId { get; set; }
    public Subclass? SubClass { get; set; }
    public int? SubClassId { get; set; }

    // Background
    public Background? Background { get; set; }
    public required int BackgroundId { get; set; }
    public CharacterInfo Info { get; set; } = new();

    // Inventory
    public required Inventory Inventory { get; set; }

    // Abilities & Combat
    public required ICollection<AbilityValue> AbilityScores { get; set; }
    public required CombatStats CombatStats { get; set; }
    public ICollection<Feat> Feats { get; set; } = [];
    public ICollection<Spell> ReadySpells { get; set; } = [];
    public int[]? CurrentSpellSlots { get; set; }
    public ICollection<ClassSlot> CurrentClassSlots { get; set; } = [];

    // Proficiencies
    public ICollection<SaveThrowProficiency> SavingThrows { get; set; } = [];
    public ICollection<DamageAffinity> DamageAffinities { get; set; } = [];
    public ICollection<SkillProficiency> SkillProficiencies { get; set; } = [];
    public ICollection<WeaponCategoryProficiency> WeaponCategoryProficiencies { get; set; } = [];
    public ICollection<WeaponTypeProficiency> WeaponTypeProficiencies { get; set; } = [];
    public ICollection<ArmorProficiency> ArmorProficiencies { get; set; } = [];
    public ICollection<ToolProficiency> ToolProficiencies { get; set; } = [];
    public ICollection<LanguageProficiency> Languages { get; set; } = [];
    public int ProficiencyBonus { get; set; } = 2;
}

[Owned]
public class CombatStats
{
    public required int MaxHP { get; set; }
    public required int CurrentHP { get; set; }
    public int TempHP { get; set; } = 0;
    public required int ArmorClass { get; set; }
    public required int Initiative { get; set; }
    public required int Speed { get; set; }
    public required int MaxHitDice { get; set; }
    public required int CurrentHitDice { get; set; }
}

[Owned]
public class CharacterInfo
{
    public string? AlignmentName { get; set; }
    public int? AlignmentId { get; set; }
    public string? PersonalityTraits { get; set; }
    public string? Ideals { get; set; }
    public string? Bonds { get; set; }
    public string? Flaws { get; set; }
    public int? Age { get; set; }
    public int? Height { get; set; }
    public int? Weight { get; set; }
    public string? Eyes { get; set; }
    public string? Skin { get; set; }
    public string? Hair { get; set; }
    public string? AlliesAndOrganizations { get; set; }
    public string? Backstory { get; set; }
    public string? CharacterPictureUrl { get; set; }
}

[Owned]
public class SaveThrowProficiency
{
    public int AbilityId { get; set; }
    public required int FeatureId { get; set; }
}

[Owned]
public class DamageAffinity
{
    public required string AffinityType { get; set; }
    public required string DamageType { get; set; }
    public required int FeatureId { get; set; }
}

[Owned]
public class SkillProficiency
{
    public int SkillId { get; set; }
    public required bool HasExpertise { get; set; }
    public required int FeatureId { get; set; }
}

[Owned]
public class WeaponCategoryProficiency
{
    public required string WeaponCategory { get; set; }
    public required int FeatureId { get; set; }
}

[Owned]
public class WeaponTypeProficiency
{
    public required string WeaponType { get; set; }
    public required int FeatureId { get; set; }
}

[Owned]
public class ArmorProficiency
{
    public required string ArmorType { get; set; }
    public required int FeatureId { get; set; }
}

[Owned]
public class ToolProficiency
{
    public required string ToolType { get; set; }
    public required int FeatureId { get; set; }
}

[Owned]
public class LanguageProficiency
{
    public int LanguageId { get; set; }
    public required int FeatureId { get; set; }
}