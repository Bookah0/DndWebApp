namespace Api.Models.DTOs.ResponseDtos;

public class SubclassResponseDto    
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int HitDie { get; set; }
    public required ICollection<ClassLevelResponseDto> ClassLevels { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public int? SpellcastingAbilityId { get; set; }
    public string? SpellcastingAbility { get; set; }
    public int ParentClassId { get; set; }
}