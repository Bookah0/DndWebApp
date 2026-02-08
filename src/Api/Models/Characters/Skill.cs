namespace Api.Models.Characters;

// From https://www.dnd5eapi.co/api/2014/skills
public class Skill : CreatableEntity
{
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    public Ability? Ability { get; set; }
    public required int AbilityId { get; set; }
}

