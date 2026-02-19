namespace Api.Models.DTOs.RequestDtos.Character;

using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.ResponseDtos;

public class CreateCharacterRequestDto
{ 
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Range(1, 20)]  
    public required int Level { get; set; }

    [Range(0, int.MaxValue)]
    public int Experience { get; set; } = 0;

    [MaxLength(100)]
    public string PlayerName { get; set; } = "";

    [Range(1, int.MaxValue)]
    public required int RaceId { get; set; }

    [Range(1, int.MaxValue)]
    public int? SubraceId { get; set; }

    [Range(1, int.MaxValue)]
    public required int ClassId { get; set; }

    [Range(1, int.MaxValue)]
    public int? SubClassId { get; set; }

    [Range(1, int.MaxValue)]
    public required int BackgroundId { get; set; }
    public CharacterInfoRequestDto? CharacterInfo { get; set; }
    public required BaseAbilityScoresDto AbilityScores { get; set; }
}

public class UpdateCharacterRequestDto
{ 
    [MaxLength(100)]
    public string? Name { get; set; }

    [Range(0, int.MaxValue)]
    public int? Experience { get; set; }

    [MaxLength(100)]
    public string? PlayerName { get; set; }
    public CharacterInfoRequestDto CharacterInfo { get; set; } = new();
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class BaseAbilityScoresDto
{
    [Range(1, 20)]
    public required int Str { get; set; }

    [Range(1, 20)]
    public required int Dex { get; set; }

    [Range(1, 20)]
    public required int Con { get; set; }

    [Range(1, 20)]
    public required int Int { get; set; }

    [Range(1, 20)]
    public required int Wis { get; set; }

    [Range(1, 20)]
    public required int Cha { get; set; }
}

public class CharacterInfoRequestDto
{
    [Range(1, int.MaxValue)]
    public int? AlignmentId { get; set; }

    [MaxLength(2000)]
    public string? PersonalityTraits { get; set; }

    [MaxLength(2000)]
    public string? Ideals { get; set; }
    
    [MaxLength(2000)]
    public string? Bonds { get; set; }
    
    [MaxLength(2000)]
    public string? Flaws { get; set; }

    [Range(1, int.MaxValue)]
    public int? Age { get; set; }
    
    [Range(1, int.MaxValue)]
    public int? Height { get; set; }

    [Range(1, int.MaxValue)]
    public int? Weight { get; set; }

    [MaxLength(500)]
    public string? Eyes { get; set; }

    [MaxLength(500)]
    public string? Skin { get; set; }

    [MaxLength(500)]
    public string? Hair { get; set; }
    
    [MaxLength(2000)]
    public string? AlliesAndOrganizations { get; set; }

    [MaxLength(2000)]
    public string? Backstory { get; set; }
    
    [Url]
    [MaxLength(2000)]
    public string? CharacterPictureUrl { get; set; }
}