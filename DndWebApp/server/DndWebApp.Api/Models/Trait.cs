using DndWebApp.Api.Utils;
using static DndWebApp.Api.Models.Class;

namespace DndWebApp.Api.Models;

public class Trait
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<Race> Races { get; set; }
    public List<SkillProficiency> SkillProficiencies { get; set; } = [];
    public List<ChoiceOption<Skill>> SkillProficiencyChoices { get; set; } = [];
    public List<Language> Languages { get; set; } = [];
    public List<ChoiceOption<Language>> LanguageChoices { get; set; } = [];
    public List<Spell> TraitSpells { get; set; } = [];
}