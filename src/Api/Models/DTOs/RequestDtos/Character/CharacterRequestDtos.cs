namespace Api.Models.DTOs.RequestDtos.Character;

using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.ResponseDtos;

public class CreateCharacterRequestDto
{ 
    public required string Name { get; set; }
    public required int Level { get; set; }
    public int? Experience { get; set; }
    public string? PlayerName { get; set; }
    public int RaceId { get; set; }
    public int? SubraceId { get; set; }
    public required int ClassId { get; set; }
    public int? SubClassId { get; set; }
    public required int BackgroundId { get; set; }
    public CharacterDescriptionRequestDto? CharacterDescription { get; set; }
    public required AbilityScoresDto AbilityScores { get; set; }
}

public class UpdateCharacterRequestDto
{ 
    public string? Name { get; set; }
    public int? Experience { get; set; }
    public string? PlayerName { get; set; }
    public CharacterDescriptionRequestDto? CharacterDescription { get; set; }
    public bool IsPublic { get; set; }
    public bool CloningAllowed { get; set; }
}

public class AbilityScoresDto
{
    [Required]
    [Range(1, 20)]
    public required int Str { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Dex { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Con { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Int { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Wis { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Cha { get; set; }
}

public class CharacterDescriptionRequestDto
{
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