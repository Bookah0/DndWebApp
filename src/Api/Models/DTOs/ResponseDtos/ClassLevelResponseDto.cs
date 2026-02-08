namespace Api.Models.DTOs.ResponseDtos;

public class ClassLevelResponseDto
{
    public int Id { get; set; }
    public required int Level { get; set; }
    public required int ProficiencyBonus { get; set; }
    public ICollection<ClassFeatureResponseDto> LevelFeatures { get; set; } = [];
    public int CantripsKnown { get; set; }
    public int SpellsKnown { get; set; }
    public int[]? SpellSlots { get; set; }
    public ICollection<ClassSlotResponseDto> ClassSlotsAtLevel { get; set; } = [];
    public required int ClassId { get; set; }
}

public class ClassSlotResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}