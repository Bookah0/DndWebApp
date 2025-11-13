using DndWebApp.Api.Models.Characters.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

// From https://www.dnd5eapi.co/api/2014/ability-scores/
public class Ability
{
    public int Id { get; set; }
    public AbilityType? Type { get; set; }
    public AbilityShortType? ShortType { get; set; }
    public required string ShortName { get; set; }
    public required string FullName { get; set; }
    public required string Description { get; set; }
    public required ICollection<Skill> Skills { get; set; }
}

public class AbilityValue
{
    public int Id { get; set; }
    public required int AbilityId { get; set; }
    public AbilityType Type { get; set; }
    public required int Value { get; set; }
}