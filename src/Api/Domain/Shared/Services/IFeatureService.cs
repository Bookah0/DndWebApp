using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Models;

namespace Api.Domain.Shared.Services;

public interface IFeatureService<T, CD, UD> where T : Feature where CD : CreateFeatureRequestDto where UD : UpdateFeatureRequestDto
{
    Task<T> GetByIdAsync(int id);
    Task<T> GetWithChoicesAsync(int id);
    Task<T> GetWithProficienciesAsync(int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> CreateAsync(CD dto);
    Task<T> UpdateAsync(UD dto, int id);
    Task DeleteAsync(int id);
    Task<T> AddSpell(int spellId, int featureId);
    Task RemoveSpell(int spellId, int featureId);
    Task<T> AddProficiency(ProficiencyRequestDto dto, int featureId);
    Task RemoveProficiency(ProficiencyRequestDto dto, int featureId);
    Task<T> AddAbilityIncrease(int abilityId, int value, int featureId);
    Task RemoveAbilityIncrease(int abilityId, int featureId);
}