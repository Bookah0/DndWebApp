namespace DndWebApp.Api.Models.DTOs.Features;


public class FeatDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsHomebrew { get; set; }
    public string Prerequisite { get; set; } = "";
    public required int FromId { get; set; }
    public required string FromType { get; set; }

    public List<int> SpellIds { get; set; } = [];
    public List<AbilityValueDto> AbilityIncreases { get; set; } = [];
    public ProficienciesDto? Proficiencies { get; set; }
    public List<DamageAffinityDto> DamageAffinities { get; set; } = [];
    public ProficiencyChoicesDto? ProficiencyChoices { get; set; }
}