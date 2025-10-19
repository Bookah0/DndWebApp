using DndWebApp.Api.Models.Items;
using DndWebApp.Api.Models.Items.Enums;
using DndWebApp.Api.Models.Spells;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Utils;

namespace DndWebApp.Api.Models.Characters;

public abstract class BenefitProvider
{
    public int Id { get; set; }
    public List<Spell> SpellsGained { get; set; } = [];
    public List<AbilityValue> AbilityIncreases { get; set; } = [];
    public List<DamageType> DamageResistance { get; set; } = [];
    public List<DamageType> DamageImmunity { get; set; } = [];

    public List<string> SkillProficiencies { get; set; } = [];
    public List<Tool> ToolProficiencies { get; set; } = [];
    public List<Language> Languages { get; set; } = [];

    public List<ChoiceOption<Skill>> SkillProficiencyChoices { get; set; } = [];
    public List<ChoiceOption<Tool>> ToolProficiencyChoices { get; set; } = [];
    public List<ChoiceOption<Language>> LanguageChoices { get; set; } = [];

}