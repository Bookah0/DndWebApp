using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.Character;

public class SubraceDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public required string GeneralDescription { get; set; }    
    
    [Required]
    [Range(1, int.MaxValue)]
    public required int ParentRaceId { get; set; }
}