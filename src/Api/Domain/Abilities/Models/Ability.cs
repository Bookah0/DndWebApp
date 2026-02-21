using Api.Domain.Skills.Models;

namespace Api.Domain.Abilities.Models;

// From https://www.dnd5eapi.co/api/2014/ability-scores/
public class Ability
{
    public int Id { get; set; }
    public required string ShortName { get; set; }
    public required string FullName { get; set; }
    public required string Description { get; set; }
    public required ICollection<Skill> Skills { get; set; }
}