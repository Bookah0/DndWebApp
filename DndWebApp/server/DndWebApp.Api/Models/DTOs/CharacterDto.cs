namespace DndWebApp.Api.Models.DTOs;

public class CharacterDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Level { get; set; }
    public required int RaceId { get; set; }
    public required int SubraceId { get; set; }
    public required int ClassId { get; set; }
    public required int? SubClassId { get; set; }
    public required int? BackgroundId { get; set; }
    public required int? Experience { get; set; }
    public required string PlayerName { get; set; }
    public required int ProficiencyBonus { get; set; }
    public CombatStatsDto? CombatStats { get; set; }
    public CharacterDescriptionDto? CharacterDescription { get; set; }
    public CharacterSpellSlotsDto? CharacterSpellSlots { get; set; }
}

public class CombatStatsDto
{
    public required int MaxHP { get; set; }
    public required int CurrentHP { get; set; }
    public required int TempHP { get; set; }
    public required int ArmorClass { get; set; }
    public required int Initiative { get; set; }
    public required int Speed { get; set; }
    public required int MaxHitDice { get; set; }
    public required int CurrentHitDice { get; set; }
}

public class CharacterDescriptionDto
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
    public required string CharacterPictureUrl { get; set; }
}

public class CharacterSpellSlotsDto
{
    public required int CharacterId { get; set; }
    public required int Lvl1 { get; set; }
    public required int Lvl2 { get; set; }
    public required int Lvl3 { get; set; }
    public required int Lvl4 { get; set; }
    public required int Lvl5 { get; set; }
    public required int Lvl6 { get; set; }
    public required int Lvl7 { get; set; }
    public required int Lvl8 { get; set; }
    public required int Lvl9 { get; set; }
}