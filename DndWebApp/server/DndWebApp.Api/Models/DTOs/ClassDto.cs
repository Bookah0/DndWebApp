using DndWebApp.Api.Models.Characters.Enums;

namespace DndWebApp.Api.Models.DTOs;

public class ClassDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HitDie { get; set; }
    public bool IsHomebrew { get; set; }
    public int? SpellLevel { get; set; }
    public int? SpellcastingAbilityId { get; set; }
}