using Api.Domain.Abilities.DTOs;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Interfaces;
using Api.Domain.Shared.Models;

namespace Api.Domain.Shared.Services;

public interface IChoiceService<T> where T : Feature
{
    Task ClearChoices<C>(int featureId) where C : IFeatureChoice;
    Task<T> AddChoice<CDto>(CDto dto, int featureId) where CDto : ChoiceDto;
    Task RemoveChoice<C>(int choiceId, int featureId) where C : IFeatureChoice;
    Task<SkillProficiencyChoice> AddSkillOptions(int choiceId, ICollection<int> newSkillIds);
    Task<LanguageChoice> AddLanguageOptions(int choiceId, ICollection<int> newLanguageIds);
    Task<AbilityIncreaseChoice> AddAbilityOptions(int choiceId, ICollection<AbilityValueDto> newAbilityValues);
    Task<WeaponCategoryProficiencyChoice> AddWeaponCategoryOptions(int choiceId, ICollection<string> newCategories);
    Task<WeaponTypeProficiencyChoice> AddWeaponTypeOptions(int choiceId, ICollection<string> newTypes);
    Task<ToolProficiencyChoice> AddToolCategoryOptions(int choiceId, ICollection<string> newCategories);
    Task<ArmorProficiencyChoice> AddArmorCategoryOptions(int choiceId, ICollection<string> newCategories);
    Task RemoveSkillOptions(int choiceId, ICollection<int> skillIdsToRemove);
    Task RemoveLanguageOptions(int choiceId, ICollection<int> languageIdsToRemove);
    Task RemoveAbilityOptions(int choiceId, ICollection<int> valuesToRemove);
    Task RemoveWeaponCategoryOptions(int choiceId, ICollection<string> categoriesToRemove);
    Task RemoveWeaponTypeOptions(int choiceId, ICollection<string> typesToRemove);
    Task RemoveToolCategoryOptions(int choiceId, ICollection<string> categoriesToRemove);
    Task RemoveArmorCategoryOptions(int choiceId, ICollection<string> categoriesToRemove);
}