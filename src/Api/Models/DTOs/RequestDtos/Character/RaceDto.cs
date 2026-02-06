using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class RaceDto
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