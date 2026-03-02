using Api.Domain.Shared.DTOs;

namespace Api.Domain.Classes.DTOs;

public class ClassResponseDto : CreatableEntityResponseDto
{
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