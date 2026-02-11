using System.ComponentModel.DataAnnotations;
using Api.Validation.AllowedValues;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateRaceRequestDto
{   
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [Range(1, 60)]        
    public int Speed { get; set; } = 30;
    
    [MaxLength(50)]
    public string Size { get; set; } = CreatureSize.Medium;

    [MinLength(1)]
    [MaxLength(2000)]
    public required SpeciesInfoDto? Info { get; set; } = new();
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
    public SpeciesInfoDto? Info { get; set; }
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
    public int Speed { get; set; } = 30;
    
    [MinLength(1)]
    [MaxLength(50)]
    public string Size { get; set; } = CreatureSize.Medium;

    [MinLength(1)]
    [MaxLength(2000)]
    public SpeciesInfoDto? Info { get; set; }    

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
    public SpeciesInfoDto? Info { get; set; }    

    [Range(1, int.MaxValue)]
    public int? NewParentRaceId { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class SpeciesInfoDto
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