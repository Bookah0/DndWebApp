using System.ComponentModel.DataAnnotations;
using Api.Models.DTOs.Features;

namespace Api.Models.DTOs.RequestDtos.Character;

public class CreateClassLevelRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public required int ClassId { get; set; }

    [Required]
    public bool IsSubclassLevel { get; set; }

    [Required]
    [Range(1, 20)]
    public required int Level { get; set; }

    [Range(1, 10)]
    public int? ProficiencyBonus { get; set; }

    [Range(0, int.MaxValue)]
    public int? CantripsKnown { get; set; }

    [Range(0, int.MaxValue)]
    public int? SpellsKnown { get; set; }
    public int[]? SpellSlots { get; set; }
    public ICollection<ClassSlotRequestDto>? ClassSpecificSlotsAtLevel { get; set; }
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
    public ICollection<ClassSlotRequestDto>? ClassSpecificSlotsAtLevel { get; set; }
}

public class ClassSlotRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
}