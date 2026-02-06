using Api.Models.Characters;
using Api.Models.DTOs.Features;
using Api.Models.Features;

namespace Api.Services.Interfaces.Features;

public interface IFeatureService<T, TD> where T : AFeature where TD : AFeatureDto
{
    Task<T> GetByIdAsync(int id);
    Task<T> GetWithChoicesAsync(int id);
    Task<T> GetWithProficienciesAsync(int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> CreateAsync(TD dto);
    Task<T> UpdateAsync(TD dto, int id);
    Task DeleteAsync(int id);
    Task<T> AddSpell(int spellId, int featureId);
    Task RemoveSpell(int spellId, int featureId);
    Task<T> AddProficiency(ProficiencyDto dto, int featureId);
    Task RemoveProficiency(ProficiencyDto dto, int featureId);
    Task<T> AddAbilityIncrease(int abilityId, int value, int featureId);
    Task RemoveAbilityIncrease(int abilityId, int featureId);
}