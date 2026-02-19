using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.Features;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateClassLevelRequestDto
{
    [Range(1, int.MaxValue)]
    public required int ClassId { get; set; }

    public bool IsSubclassLevel { get; set; }

    [Range(1, 20)]
    public required int Level { get; set; }

    [Range(0, int.MaxValue)]
    public int CantripsKnown { get; set; } = 0;

    [Range(0, int.MaxValue)]
    public int SpellsKnown { get; set; } = 0;
    public int[] SpellSlots { get; set; } = [];
}

public class UpdateClassLevelRequestDto
{
    [Range(1, int.MaxValue)]
    public int? NewClassId { get; set; }
    public bool? NewClassIsSubclass { get; set; }

    [Range(1, 20)]
    public int? Level { get; set; }

    [Range(1, 10)]
    public int? ProficiencyBonus { get; set; }

    [Range(0, int.MaxValue)]
    public int? CantripsKnown { get; set; }

    [Range(0, int.MaxValue)]
    public int? SpellsKnown { get; set; }
    public int[]? SpellSlots { get; set; }
}

public class ClassSlotRequestDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
}