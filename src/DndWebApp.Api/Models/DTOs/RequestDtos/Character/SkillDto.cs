using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.Character;

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