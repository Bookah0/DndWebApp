using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateLanguageRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Family { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string Script { get; set; } = "";

    [MinLength(1)]
    [MaxLength(1000)]
    public string TypicalSpeakers { get; set; } = "";
}

public class UpdateLanguageRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string? Family { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public string? Script { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? TypicalSpeakers { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}