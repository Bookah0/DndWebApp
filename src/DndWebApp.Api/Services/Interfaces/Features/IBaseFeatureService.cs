using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IBaseFeatureService<T> where T : AFeature
{
    Task<T> CreateAsync(AFeatureDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task UpdateAsync(int id, AFeatureDto dto);
    Task AddSpell(int spellId, int featureId);
    Task RemoveSpell(int spellId, int featureId);
    Task AddProficiency(ProficiencyDto dto, int featureId);
    Task RemoveProficiency(ProficiencyDto dto, int featureId);
    Task AddAbilityIncrease(int abilityId, int value, int featureId);
    Task RemoveAbilityIncrease(int abilityId, int featureId);
}