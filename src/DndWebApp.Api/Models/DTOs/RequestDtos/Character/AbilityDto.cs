using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.RequestDtos.Character;

public class AbilityDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public required string ShortName { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string FullName { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }
}

public class AbilityValueDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int AbilityId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(30)]
    public required int Value { get; set; }
}