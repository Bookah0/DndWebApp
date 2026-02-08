using Api.Models.Features;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.Characters;

// Based on https://api.open5e.com/v1/races/ & https://www.dnd5eapi.co/api/2014/races/
public class Species : CreatableEntity
{
    public required string Name { get; set; }
    public SpeciesDescriptions RaceDescription { get; set; } = new();
    public required int Speed { get; set; }
    public string Size { get; set; } = "Medium";
    public ICollection<Trait> Traits { get; set; } = [];
}

public class Race : Species
{
    public ICollection<Subrace> SubRaces { get; set; } = [];
}

public class Subrace : Species
{
    public required Race ParentRace { get; set; }
    public required int ParentRaceId { get; set; }
}

[Owned]
public class SpeciesDescriptions
{
    public string General { get; set; } = "";
    public string Aging { get; set; } = "";
    public string CommonAlignment { get; set; } = "";
    public string Size { get; set; } = "";
    public string Languages { get; set; } = "";
}