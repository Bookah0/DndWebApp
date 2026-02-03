using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IFeatureService<T, TD> where T : AFeature where TD : AFeatureDto
{
    Task<T> GetByIdAsync(int id);
    Task<T> GetWithChoicesAsync(int id);
    Task<T> GetWithProficienciesAsync(int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> CreateAsync(TD dto);
    Task UpdateAsync(TD dto, int id);
    Task DeleteAsync(int id);
    Task AddSpell(int spellId, int featureId);
    Task RemoveSpell(int spellId, int featureId);
    Task AddProficiency(ProficiencyDto dto, int featureId);
    Task RemoveProficiency(ProficiencyDto dto, int featureId);
    Task AddAbilityIncrease(int abilityId, int value, int featureId);
    Task RemoveAbilityIncrease(int abilityId, int featureId);
}