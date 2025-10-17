using DndWebApp.Api.Enums;
using DndWebApp.Api.Models;
using DndWebApp.Api.Utils;
using Microsoft.AspNetCore.Components.Forms;

namespace DndWebApp.Api.Models;

public class Character
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required List<Race> Species { get; set; }
    public required Class Class { get; set; }
    public required Class SubClass { get; set; }
    public required Background Background { get; set; }
    public required int Level { get; set; }
    public List<Feature> FeaturesAndTraits { get; set; } = [];
    public List<Spell> ReadySpells { get; set; } = [];
    public string PlayerName { get; set; } = "";

    // Combat Stats
    public required int MaxHP { get; set; }
    public required int CurrentHP { get; set; }
    public int TempHP { get; set; } = 0;
    public required int ArmorClass { get; set; }
    public required int Initiative { get; set; }
    public required int Speed { get; set; }
    public required int ProficiencyBonus { get; set; }
    public required int MaxHitDice { get; set; }
    public required int CurrentHitDice { get; set; }

    // Proficiencies
    
    public List<SavingThrowProficiency> SaveProficienies { get; set; } = [];
    public List<SkillProficiency> SkillProficienies { get; set; } = [];
    public List<SkillExpertise> SkillExpertise { get; set; } = [];
    public List<Language> Languages { get; set; } = [];
    public List<string> EquipmentProficiencies { get; set; } = [];

    public required List<AbilityValue> AbilityScores { get; set; }
    public CurrentSpellSlots? CurrentSpellSlots { get; set; }

    // Character building
    public string Alignment { get; set; } = "";
    public string PersonalityTraits { get; set; } = "";
    public string Ideals { get; set; } = "";
    public string Bonds { get; set; } = "";
    public string Flaws { get; set; } = "";
    public int? Age { get; set; }
    public string Height { get; set; } = "";
    public string Weight { get; set; } = "";
    public string Eyes { get; set; } = "";
    public string Skin { get; set; } = "";
    public string Hair { get; set; } = "";
    public Stream? CharacterPicture { get; set; }
    public string AlliesAndOrganizations { get; set; } = "";
    public string Backstory { get; set; } = "";
}

public class CurrentSpellSlots
{
    public required int Lvl1 { get; set; }
    public int Lvl2 { get; set; } = 0;
    public int Lvl3 { get; set; } = 0;
    public int Lvl4 { get; set; } = 0;
    public int Lvl5 { get; set; } = 0;
    public int Lvl6 { get; set; } = 0;
    public int Lvl7 { get; set; } = 0;
    public int Lvl8 { get; set; } = 0;
    public int Lvl9 { get; set; } = 0;
}