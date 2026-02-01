using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.RequestDtos.Character;

public class ClassLevelDto
{
    [Required]
    [Range(1, 20)]
    public required int Level { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ProficiencyBonus { get; set; }

    public ICollection<int> NewFeatureIds { get; set; } = [];

    [Range(0, int.MaxValue)]
    public int CantripsKnown { get; set; }

    [Range(0, int.MaxValue)]
    public int SpellsKnown { get; set; }

    public int[]? SpellSlotsAtLevel { get; set; }

    public ICollection<ClassSpecificSlotDto> ClassSpecificSlotsAtLevel { get; set; } = [];

    [Required]
    public required bool IsSubclassLevel { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int ClassId { get; set; }
}

public class ClassSpecificSlotDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
}