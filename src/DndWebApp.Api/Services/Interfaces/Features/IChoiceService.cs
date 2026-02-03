using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Repositories.Interfaces;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IChoiceService<out T> where T : AFeature
{
    Task ClearChoices<C>(int featureId) where C : IFeatureChoice;
    Task AddChoice<CDto>(CDto dto, int featureId) where CDto : AChoiceDto;
    Task RemoveChoice<C>(int choiceId, int featureId) where C : IFeatureChoice;
    Task AddSkillOptions(int choiceId, ICollection<int> newSkillIds);
    Task AddLanguageOptions(int choiceId, ICollection<int> newLanguageIds);
    Task AddAbilityOptions(int choiceId, ICollection<AbilityValueDto> newAbilityValues);
    Task AddWeaponCategoryOptions(int choiceId, ICollection<string> newCategories);
    Task AddWeaponTypeOptions(int choiceId, ICollection<string> newTypes);
    Task AddToolCategoryOptions(int choiceId, ICollection<string> newCategories);
    Task AddArmorCategoryOptions(int choiceId, ICollection<string> newCategories);
    Task RemoveSkillOptions(int choiceId, ICollection<int> skillIdsToRemove);
    Task RemoveLanguageOptions(int choiceId, ICollection<int> languageIdsToRemove);
    Task RemoveAbilityOptions(int choiceId, ICollection<AbilityValueDto> valuesToRemove);
    Task RemoveWeaponCategoryOptions(int choiceId, ICollection<string> categoriesToRemove);
    Task RemoveWeaponTypeOptions(int choiceId, ICollection<string> typesToRemove);
    Task RemoveToolCategoryOptions(int choiceId, ICollection<string> categoriesToRemove);
    Task RemoveArmorCategoryOptions(int choiceId, ICollection<string> categoriesToRemove);
}