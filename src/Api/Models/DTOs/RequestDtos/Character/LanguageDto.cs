using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class LanguageDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Family { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Script { get; set; }

    [Required]
    public required bool IsHomebrew { get; set; }
}