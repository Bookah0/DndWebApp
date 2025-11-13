using DndWebApp.Api.Models.Characters.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

// From https://www.dnd5eapi.co/api/2014/skills
public class Skill
{
    public int Id { get; set; }
    public SkillType? Type { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public Ability? Ability { get; set; }
    public required int AbilityId { get; set; }
    public bool IsHomebrew { get; set; } = false;
}

