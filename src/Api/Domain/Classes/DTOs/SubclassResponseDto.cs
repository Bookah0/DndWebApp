namespace Api.Domain.Classes.DTOs;

public class SubclassResponseDto    
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int HitDie { get; set; }
    public required ICollection<ClassLevelResponseDto> ClassLevels { get; set; }
    public int? SpellcastingAbilityId { get; set; }
    public string? SpellcastingAbility { get; set; }
    public int ParentClassId { get; set; }
}