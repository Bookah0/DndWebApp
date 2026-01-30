using DndWebApp.Api.Models.DTOs.Character;

namespace DndWebApp.Api.Models.DTOs.ResponseDtos;

public class SkillResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required AbilityDto Ability { get; set; }
    public required bool IsHomebrew { get; set; }
}