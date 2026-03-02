using System.ComponentModel.DataAnnotations;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Backgrounds.DTOs;

public class CreateBackgroundRequestDto : CreateableEntityRequestDto
{  
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public required string Description { get; set; }
    
    public required CurrencyDto StartingCurrency { get; set; }
}

public class UpdateBackgroundRequestDto : CreateableEntityRequestDto
{  
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public string? Description { get; set; }
    public CurrencyDto? StartingCurrency { get; set; }
}

public class StartingItemOptionDto
{
    [MinLength(1)]
    [MaxLength(500)]
    public required string Description { get; set; }

    public required ICollection<int> ItemOptionIds { get; set; }
}