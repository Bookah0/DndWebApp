using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models;

public class Race
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int Speed { get; set; }
    public List<AbilityValue> AbilityBonuses { get; set; } = [];
    public List<SkillProficiency> SkillProficiencies { get; set; } = [];
    public string AgingDescription { get; set; } = "";
    public string Alignment { get; set; } = "";
    public required string Size { get; set; }
    public string SizeDescription { get; set; } = "";
    public required List<Language> Languages { get; set; }
    public string LanguageDescription { get; set; } = "";
    public required List<Feature> SpeciesTraits { get; set; }
    public required List<Race> SubRaces { get; set; }

}