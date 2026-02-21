using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Alignments.DTOs;

public class AlignmentRequestDto
{
    [MinLength(1)]
    [MaxLength(50)]
    public required string Name { get; set; }

    [MinLength(1)]
    [MaxLength(10)]
    public required string Abbreviation { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }
}