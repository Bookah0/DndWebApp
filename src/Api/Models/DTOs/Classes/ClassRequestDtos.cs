using System.ComponentModel.DataAnnotations;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateClassRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Required]
    [Range(1, 20)]
    public required int HitDie { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int? SpellcastingAbilityId { get; set; }
}

public class UpdateClassRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public string? Description { get; set; }

    [Range(1, 20)]
    public int? HitDie { get; set; }

    [Range(1, int.MaxValue)]
    public int? SpellcastingAbilityId { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

public class CreateSubclassRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public required string Description { get; set; }

    [Required]
    [Range(1, 20)]
    public required int HitDie { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int? SpellcastingAbilityId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ParentClassId { get; set; }
}

public class UpdateSubclassRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(1)]
    [MaxLength(2000)]
    public string? Description { get; set; }

    [Range(1, 20)]
    public int? HitDie { get; set; }

    [Range(1, int.MaxValue)]
    public int? SpellcastingAbilityId { get; set; }

    [Range(1, int.MaxValue)]
    public int? NewParentClassId { get; set; }
    public bool? IsPublic { get; set; }
    public bool? CloningAllowed { get; set; }
}

