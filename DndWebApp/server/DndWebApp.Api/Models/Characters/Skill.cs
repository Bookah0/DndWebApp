using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

// From https://www.dnd5eapi.co/api/2014/skills
public class Skill
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int AbilityId { get; set; }
    public bool IsHomebrew { get; set; } = false;
}

[Owned]
public class SkillProficiency
{
    public required int SkillId { get; set; }
    public required bool HasExpertise { get; set; }
    public required int CharacterFeatureId { get; set; }
}