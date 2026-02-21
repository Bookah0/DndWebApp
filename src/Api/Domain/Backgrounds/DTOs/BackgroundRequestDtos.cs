using System.ComponentModel.DataAnnotations;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Backgrounds.DTOs;

public class CreateBackgroundRequestDto
{  
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public required string Description { get; set; }
    
    public required CurrencyDto StartingCurrency { get; set; }
}

public class UpdateBackgroundRequestDto
{  
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public string? Description { get; set; }
    public CurrencyDto? StartingCurrency { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class StartingItemOptionDto
{
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    public required ICollection<int> ItemOptionIds { get; set; }
}