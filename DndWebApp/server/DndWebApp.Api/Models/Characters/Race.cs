

using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Models.Characters;

// Based on https://api.open5e.com/v1/races/ & https://www.dnd5eapi.co/api/2014/races/

public class Race
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public bool IsHomebrew { get; set; } = false;
    public required int Speed { get; set; }
    public List<AbilityValue> AbilityBonuses { get; set; } = [];
    public List<Skill> SkillProficiencies { get; set; } = [];
    public string AgingDescription { get; set; } = "";
    public string CommonAlignmentDescription { get; set; } = "";
    public required CreatureSize Size { get; set; }
    public string SizeDescription { get; set; } = "";
    public required List<Language> Languages { get; set; }
    public string LanguageDescription { get; set; } = "";
    public required List<Trait> SpeciesTraits { get; set; }
    public List<Race> SubRaces { get; set; } = [];
    public Race? ParentRace { get; set; }

}