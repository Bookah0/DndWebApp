namespace DndWebApp.Api.Models.Characters;

// From https://www.dnd5eapi.co/api/2014/skills
public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Ability { get; set; }
    public bool IsHomebrew { get; set; } = false;
}

public class SkillProficiency
{
    public int Id { get; set; }
    public required string Skill { get; set; }
    public required bool IsProficient { get; set; }
    public required bool HasExpertise { get; set; }
}