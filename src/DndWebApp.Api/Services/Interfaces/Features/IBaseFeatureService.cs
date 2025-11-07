using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Models.Items.Enums;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IBaseFeatureService<T> where T : AFeature
{
    Task AddSpell(int spellId, int featureId);
    Task RemoveSpell(int spellId, int featureId);
    Task AddProficiency<TEnum>(TEnum proficiency, int featureId) where TEnum : struct, Enum;
    Task RemoveProficiency<TEnum>(TEnum proficiency, int featureId) where TEnum : struct, Enum;
    Task AddDamageAffinity(AffinityType affinityType, DamageType damageType, int featureId);
    Task RemoveDamageAffinity(AffinityType affinityType, DamageType damageType, int featureId);
    Task AddAbilityIncrease(int abilityId, int value, int featureId);
    Task RemoveAbilityIncrease(int abilityId, int featureId);
    Task AddAbilityIncreaseChoice(List<AbilityValue> options, string description, int featureId);
    Task RemoveAbilityIncreaseChoice(int choiceId, int featureId);
    Task AddProficiencyChoice<TEnum>(List<TEnum> options, string description, int featureId) where TEnum : struct, Enum;
    Task RemoveProficiencyChoice<TEnum>(int choiceId, int featureId) where TEnum : struct, Enum;
}