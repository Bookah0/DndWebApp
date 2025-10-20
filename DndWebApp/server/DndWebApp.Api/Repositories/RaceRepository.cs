

using DndWebApp.Api.Models.Characters.Enums;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Models.Characters;

// Based on https://api.open5e.com/v1/races/ & https://www.dnd5eapi.co/api/2014/races/
public class Race
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public RaceDescription? RaceDescription { get; set; }
    public bool IsHomebrew { get; set; } = false;

    public required int Speed { get; set; }
    public required ICollection<Trait> Traits { get; set; }
    public ICollection<Race> SubRaces { get; set; } = [];
    public Race? ParentRace { get; set; }
    public int? ParentRaceId { get; set; }
}

[Owned]
public class RaceDescription
{
    public string AgingDescription { get; set; } = "";
    public string CommonAlignmentDescription { get; set; } = "";
    public CreatureSize Size { get; set; } = CreatureSize.Medium;
    public string SizeDescription { get; set; } = "";
    public string LanguageDescription { get; set; } = "";
}