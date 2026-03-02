using System.ComponentModel.DataAnnotations;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Classes.DTOs;

public class CreateClassRequestDto : CreateClassRequestBaseDto
{
}

public class UpdateClassRequestDto : UpdateClassRequestBaseDto
{
}

public class CreateSubclassRequestDto : CreateClassRequestBaseDto
{
    [Range(1, int.MaxValue)]
    public required int ParentClassId { get; set; }
}

public class UpdateSubclassRequestDto : UpdateClassRequestBaseDto
{
    [Range(1, int.MaxValue)]
    public int? NewParentClassId { get; set; }
}

public abstract class CreateClassRequestBaseDto : CreateableEntityRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Range(1, 20)]
    public required int HitDie { get; set; }

    [Range(1, int.MaxValue)]
    public required int? SpellcastingAbilityId { get; set; }
}

public class UpdateClassRequestBaseDto : CreateableEntityRequestDto
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
}


