using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IBaseFeatureService<T> where T : AFeature
{
    Task AddSpell(int spellId, int featureId);
    Task RemoveSpell(int spellId, int featureId);
    Task AddProficiency(ProficiencyDto dto, int featureId);
    Task RemoveProficiency(ProficiencyDto dto, int featureId);
    Task AddAbilityIncrease(int abilityId, int value, int featureId);
    Task RemoveAbilityIncrease(int abilityId, int featureId);
    Task ClearAbilityIncreaseChoices(int featureId);
    Task AddProficiencyChoice(ProficiencyChoiceDto dto, int featureId);
    Task AddProficiencyChoice(AbilityIncreaseChoiceDto dto, int featureId);
    Task RemoveProficiencyChoice(string type, int choiceIndex, int featureId);
}