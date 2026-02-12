namespace Api.Models.DTOs.ResponseDtos;

public class ClassResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int HitDie { get; set; }
    public required ICollection<ClassLevelResponseDto> ClassLevels { get; set; }
    public int? SpellcastingAbilityId { get; set; }
    public string? SpellcastingAbility { get; set; }
    public ICollection<int> SubclassIds { get; set; } = [];
    public ICollection<int> StartingEquipmentIds { get; set; } = [];
    public ICollection<int> StartingEquipmentChoiceIds { get; set; } = [];
}