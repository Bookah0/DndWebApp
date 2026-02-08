using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateRaceRequestDto
{   
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [Range(1, 60)]        
    public int? Speed { get; set; }
    
    [MaxLength(50)]
    public string? Size { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public required SpeciesDescriptionsDto? SpeciesDescriptions { get; set; }    
}

public class UpdateRaceRequestDto
{   
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [Range(1, 60)]        
    public int? Speed { get; set; }
    
    [MaxLength(50)]
    public string? Size { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public SpeciesDescriptionsDto? SpeciesDescriptions { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class CreateSubraceRequestDto
{   
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ParentRaceId { get; set; }

    [Range(1, 60)]        
    public int? Speed { get; set; }
    
    [MinLength(1)]
    [MaxLength(50)]
    public string Size { get; set; } = "Medium";

    [MinLength(1)]
    [MaxLength(2000)]
    public SpeciesDescriptionsDto? SpeciesDescriptions { get; set; }    

}

public class UpdateSubraceRequestDto
{   
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [Range(1, 60)]        
    public int? Speed { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string? Size { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public SpeciesDescriptionsDto? SpeciesDescriptions { get; set; }    

    [Range(1, int.MaxValue)]
    public int? NewParentRaceId { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class SpeciesDescriptionsDto
{
    [MaxLength(4000)]
    public string? GeneralDescription { get; set; }

    [MaxLength(2000)]
    public string? AgingDescription { get; set; }
    
    [MaxLength(2000)]
    public string? AlignmentDescription { get; set; }

    [MaxLength(2000)]
    public string? SizesDescription { get; set; }
    
    [MaxLength(2000)]
    public string? LanguagesDescription { get; set; }
}