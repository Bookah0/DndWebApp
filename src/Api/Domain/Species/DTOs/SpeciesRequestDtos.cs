using System.ComponentModel.DataAnnotations;
using Api.Domain.Shared.Enums.Character;

namespace Api.Domain.Species.DTOs;

public class CreateRaceRequestDto
{   
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    public required int Speed { get; set; } = 30;
    
    [MaxLength(50)]
    public string Size { get; set; } = CreatureSize.Medium;

    public SpeciesInfoDto? Info { get; set; } = new();
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
    public SpeciesInfoDto? Info { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class CreateSubraceRequestDto
{   
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Range(1, int.MaxValue)]
    public required int ParentRaceId { get; set; }

    [Range(1, 60)]        
    public int Speed { get; set; } = 30;
    
    [MaxLength(50)]
    public string Size { get; set; } = CreatureSize.Medium;
    public SpeciesInfoDto? Info { get; set; } = new();

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
    public SpeciesInfoDto? Info { get; set; }    

    [Range(1, int.MaxValue)]
    public int? NewParentRaceId { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class SpeciesInfoDto
{
    [MaxLength(4000)]
    public string? General { get; set; }

    [MaxLength(2000)]
    public string? Aging { get; set; }
    
    [MaxLength(2000)]
    public string? CommonAlignment { get; set; }

    [MaxLength(2000)]
    public string? Size { get; set; }
    
    [MaxLength(2000)]
    public string? Languages { get; set; }
}