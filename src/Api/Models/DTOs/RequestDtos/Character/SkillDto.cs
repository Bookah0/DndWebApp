using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class SkillDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int AbilityId { get; set; }

    [Required]
    public required bool IsHomebrew { get; set; }
}