namespace DndWebApp.Api.Models.DTOs;

public class CharacterDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Level { get; set; }
    public required int RaceId { get; set; }
    public int? SubraceId { get; set; }
    public required int ClassId { get; set; }
    public int? SubClassId { get; set; }
    public required int BackgroundId { get; set; }
    public required string PlayerName { get; set; }
    public required AbilityScoresDto AbilityScores { get; set; }
    public CharacterDescriptionDto? CharacterDescription { get; set; }
}

public class AbilityScoresDto
{
    public required int Strength { get; set; }
    public required int Dexterity { get; set; }
    public required int Constitution { get; set; }
    public required int Intelligence { get; set; }
    public required int Wisdom { get; set; }
    public required int Charisma { get; set; }
}

public class CharacterDescriptionDto
{
    public int Id { get; set; }
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