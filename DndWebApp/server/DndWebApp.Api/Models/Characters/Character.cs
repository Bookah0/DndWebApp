using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

public class Character
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required List<Race> Races { get; set; }
    public required Class Class { get; set; }
    public required Class SubClass { get; set; }
    public required Background Background { get; set; }
    public required int Level { get; set; }
    public int? Experience { get; set; }
    public string PlayerName { get; set; } = "";
    
    public List<Feature> Features { get; set; } = [];
    public List<Trait> Traits { get; set; } = [];
    public List<Feat> Feats { get; set; } = [];
    public List<Spell> ReadySpells { get; set; } = [];

    // Combat Stats
    public required int MaxHP { get; set; }
    public required int CurrentHP { get; set; }
    public int TempHP { get; set; } = 0;
    public required int ArmorClass { get; set; }
    public required int Initiative { get; set; }
    public required int Speed { get; set; }
    public required int MaxHitDice { get; set; }
    public required int CurrentHitDice { get; set; }

    // Proficiencies
    public required int ProficiencyBonus { get; set; }
    public List<Ability> SaveProficienies { get; set; } = [];
    public List<SkillProficiency> SkillProficienies { get; set; } = [];
    public List<Language> Languages { get; set; } = [];
    public List<ItemCategory> EquipmentProficiencies { get; set; } = [];

    public required List<AbilityValue> AbilityScores { get; set; }
    public CurrentSpellSlots? CurrentSpellSlots { get; set; }
    public CharacterBuilding CharacterBuildData { get; set; } = new();

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

    // public string? CharacterPictureUrl { get; set; }
}