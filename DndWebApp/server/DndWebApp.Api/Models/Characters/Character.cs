using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

public class Character
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int Level { get; set; }

    public required Race Race { get; set; }
    public ICollection<Race> OtherRaces { get; set; } = [];

    public required Class Class { get; set; }
    public Class? SubClass { get; set; }

    public required Background Background { get; set; }
    
    public int? Experience { get; set; }
    public string PlayerName { get; set; } = "";
    
    public required ICollection<AbilityValue> AbilityScores { get; set; }
    public required CombatStats CombatStats { get; set; }
    public required CharacterProficiencies CharacterProficiencies { get; set; }
    public ICollection<CharacterFeature> Features { get; set; } = [];
    public ICollection<Spell> ReadySpells { get; set; } = [];
    public CurrentSpellSlots? CurrentSpellSlots { get; set; }
    public CharacterBuilding CharacterBuildData { get; set; } = new();

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

    public ICollection<DamageAffinity> DamageAffinities { get; set; } = [];
}

[Owned]
public class CharacterProficiencies
{
    public ICollection<SaveThrowProficiency> SavingThrows { get; set; } = [];
    public ICollection<SkillProficiency> SkillProficiencies { get; set; } = [];
    public ICollection<WeaponProficiency> WeaponProficiencies { get; set; } = [];
    public ICollection<ArmorProficiency> ArmorProficiencies { get; set; } = [];
    public ICollection<ToolProficiency> ToolProficiencies { get; set; } = [];
    public ICollection<LanguageProficiency> Languages { get; set; } = [];
    public int ProficiencyBonus { get; set; } = 2;
}

[Owned]
public class CharacterBuilding
{
    public Alignment? Alignment { get; set; }
    public string PersonalityTraits { get; set; } = "";
    public string Ideals { get; set; } = "";
    public string Bonds { get; set; } = "";
    public string Flaws { get; set; } = "";
    public int? Age { get; set; }
    public int? Height { get; set; }
    public int? Weight { get; set; }
    public string Eyes { get; set; } = "";
    public string Skin { get; set; } = "";
    public string Hair { get; set; } = "";
    public string AlliesAndOrganizations { get; set; } = "";
    public string Backstory { get; set; } = "";
    public string? CharacterPictureUrl { get; set; }
}