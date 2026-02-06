using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class AlignmentDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public required string Abbreviation { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }
}