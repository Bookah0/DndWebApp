namespace Api.Models.DTOs.ResponseDtos;

public class CharacterResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Level { get; set; }
    public int? Experience { get; set; }
    public string PlayerName { get; set; } = "";
    public required string TimeCreated { get; set; }

    // Races & Subraces
    public int RaceId { get; set; }
    public int? SubraceId { get; set; }

    // Classes & Subclasses
    public required int ClassId { get; set; }
    public int? SubClassId { get; set; }

    // Background
    public required int BackgroundId { get; set; }
    public CharacterInfoResponseDto CharacterInfo { get; set; } = new();

    // Inventory
    public required int InventoryId { get; set; }

    // Abilities & Combat
    public required ICollection<AbilityValueResponseDto> AbilityScores { get; set; }
    public required CombatStatsResponseDto CombatStats { get; set; }
    public ICollection<int> ReadySpellIds { get; set; } = [];
    public int[]? CurrentSpellSlots { get; set; }
    public ICollection<ClassFeatureResponseDto> CurrentClassSlots { get; set; } = [];

    // Proficiencies
    public required ProficienciesResponseDto Proficiencies { get; set; }
    public int ProficiencyBonus { get; set; } = 2;
}

public class CombatStatsResponseDto
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

public class CharacterInfoResponseDto
{
    public int? AlignmentId { get; set; }
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