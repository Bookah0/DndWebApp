using Api.Domain.Abilities.DTOs;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Skills.DTOs;

public class SkillResponseDto : CreatableEntityResponseDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required AbilityRequestDto Ability { get; set; }
}