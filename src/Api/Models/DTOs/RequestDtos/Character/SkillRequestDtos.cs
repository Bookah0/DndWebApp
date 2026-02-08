using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateSkillRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int AbilityId { get; set; }
}

public class UpdateSkillRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int? NewAbilityId { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}