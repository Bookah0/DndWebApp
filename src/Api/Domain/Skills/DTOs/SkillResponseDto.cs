using Api.Domain.Abilities.DTOs;

namespace Api.Domain.Skills.DTOs;

public class SkillResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required AbilityRequestDto Ability { get; set; }
}