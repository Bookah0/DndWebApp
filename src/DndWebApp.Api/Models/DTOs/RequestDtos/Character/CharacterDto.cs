namespace DndWebApp.Api.Models.DTOs.RequestDtos.Character;

using System.ComponentModel.DataAnnotations;

public class CharacterDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Range(1, 20)]
    public int Level { get; set; }

    [Range(1, int.MaxValue)]
    public int RaceId { get; set; }

    [Range(1, int.MaxValue)]
    public int? SubraceId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ClassId { get; set; }

    [Range(1, int.MaxValue)]
    public int? SubClassId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int BackgroundId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string PlayerName { get; set; }

    [Required]
    public required AbilityScoresDto AbilityScores { get; set; }

    public CharacterDescriptionDto? CharacterDescription { get; set; }
}

public class AbilityScoresDto
{
    [Required]
    [Range(1, 20)]
    public required int Strength { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Dexterity { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Constitution { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Intelligence { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Wisdom { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Charisma { get; set; }
}

public class CharacterDescriptionDto
{
    [Range(1, int.MaxValue)]
    public required int? AlignmentId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string PersonalityTraits { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Ideals { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Bonds { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string Flaws { get; set; }

    [Range(1, 150)]
    public required int? Age { get; set; }

    [Range(1, 300)]
    public required int? Height { get; set; }

    [Range(1, 500)]
    public required int? Weight { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string Eyes { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string Skin { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string Hair { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public required string AlliesAndOrganizations { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public required string Backstory { get; set; }

    [Required]
    [Url]
    public required string CharacterPictureUrl { get; set; }
}