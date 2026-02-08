using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Models.DTOs.Features;


public abstract class CreateFeatureRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }
}

public abstract class UpdateFeatureRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public string? Description { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class CreateFeatRequestDto : CreateFeatureRequestDto
{
    [MinLength(1)]
    [MaxLength(500)]
    public string Prerequisite { get; set; } = "";

    [Range(1, int.MaxValue)]
    public int? FromClassId { get; set; }

    [Range(1, int.MaxValue)]
    public int? FromRaceId { get; set; }

    [Range(1, int.MaxValue)]
    public int? FromBackgroundId { get; set; }
}

public class UpdateFeatRequestDto : UpdateFeatureRequestDto
{
    [MinLength(1)]
    [MaxLength(500)]
    public string? Prerequisite { get; set; }

    [MinLength(1)]
    [MaxLength(10)]
    public string? NewFromType { get; set; }

    [Range(1, int.MaxValue)]
    public int? NewFromId { get; set; }
}

public class CreateBackgroundFeatureRequestDto : CreateFeatureRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int BackgroundId { get; set; }
}

public class UpdateBackgroundFeatureRequestDto : UpdateFeatureRequestDto
{
    [Range(1, int.MaxValue)]
    public int? NewBackgroundId { get; set; }
}

public class CreateClassFeatureRequestDto : CreateFeatureRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int LevelId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ClassId { get; set; }
}

public class UpdateClassFeatureRequestDto : UpdateFeatureRequestDto
{
    [Range(1, int.MaxValue)]
    public int? NewLevelId { get; set; }

    [Range(1, int.MaxValue)]
    public int? NewClassId { get; set; }
}

public class CreateTraitRequestDto : CreateFeatureRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int RaceId { get; set; }
}

public class UpdateTraitRequestDto : UpdateFeatureRequestDto
{
    [Range(1, int.MaxValue)]
    public int? NewRaceId { get; set; }
}
