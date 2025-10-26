using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;

namespace DndWebApp.Api.Models.DTOs;

public class SkillDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int AbilityId { get; set; }
    public required bool IsHomebrew { get; set; }
}