using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.RequestDtos.Character;

public class SubraceDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    public bool IsHomebrew { get; set; } = false;
    
    [Required]
    [Range(1, 60)]        
    public required int Speed { get; set; }
    
    [MaxLength(50)]
    public string Size { get; set; } = "Medium";

    [Required]
    [Range(1, int.MaxValue)]
    public required int ParentRaceId { get; set; }

    [Range(1, int.MaxValue)]
    public int? NewParentRaceId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public required string GeneralDescription { get; set; }    
    
    [MaxLength(2000)]
    public string? AgingDescription { get; set; }
    
    [MaxLength(2000)]
    public string? CommonAlignmentDescription { get; set; }

    [MaxLength(2000)]
    public string? SizeDescription { get; set; }
    
    [MaxLength(2000)]
    public string? LanguageDescription { get; set; }
}