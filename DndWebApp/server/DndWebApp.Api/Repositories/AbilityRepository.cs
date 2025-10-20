using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;


// From https://www.dnd5eapi.co/api/2014/ability-scores/
public class Ability
{
    public required int Id { get; set; }
    public required string ShortName { get; set; }
    public required string FullName { get; set; }
    public required string Description { get; set; }
    public required ICollection<Skill> Skills { get; set; }
}

public class AbilityValue
{
    public required Ability Ability { get; set; }
    public required int AbilityId { get; set; }
    public required int Value { get; set; }
}

[Owned]
public class SaveThrowProficiency
{
    public required int AbilityId { get; set; }
    public required int CharacterFeatureId { get; set; }
}