namespace Api.Domain.Abilities.DTOs;

using System.ComponentModel.DataAnnotations;

public class AbilityRequestDto
{
    [MinLength(1)]
    [MaxLength(10)]
    public required string ShortName { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    public required string FullName { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }
}

public class AbilityValueDto
{
    [Range(1, int.MaxValue)]
    public required int AbilityId { get; set; }

    [MinLength(1)]
    [MaxLength(30)]
    public required int Value { get; set; }
}