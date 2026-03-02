

using Api.Domain.Abilities.DTOs;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Characters.DTOs;

public class CharacterResponseDto : CreatableEntityResponseDto
{
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
    public required CharacterInfoResponseDto Info { get; set; }

    // Inventory
    public required int InventoryId { get; set; }

    // Abilities & Combat
    public required ICollection<AbilityValueDto> AbilityScores { get; set; }
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
    public required int? AlignmentId { get; set; }
    public required string PersonalityTraits { get; set; }
    public required string Ideals { get; set; }
    public required string Bonds { get; set; }
    public required string Flaws { get; set; }
    public required int? Age { get; set; }
    public required int? Height { get; set; }
    public required int? Weight { get; set; }
    public required string Eyes { get; set; }
    public required string Skin { get; set; }
    public required string Hair { get; set; }
    public required string AlliesAndOrganizations { get; set; }
    public required string Backstory { get; set; }
    public required string? CharacterPictureUrl { get; set; }
}