using System.ComponentModel.DataAnnotations;

namespace DndWebApp.Api.Models.DTOs.Character;

public class ClassDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Required]
    [Range(1, 20)]
    public required int HitDie { get; set; }

    public bool IsHomebrew { get; set; }

    [Range(1, int.MaxValue)]
    public int? SpellcastingAbilityId { get; set; }

    public List<int> ClassLevelIds { get; set; } = [];
}