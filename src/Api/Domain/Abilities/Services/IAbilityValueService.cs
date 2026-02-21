using Api.Domain.Abilities.DTOs;
using Api.Domain.Abilities.Models;

namespace Api.Domain.Abilities.Services;

public interface IAbilityValueService
{
    Task<AbilityValue> CreateAsync(AbilityValueDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<AbilityValue>> GetAllAsync();
    Task<AbilityValue> GetByIdAsync(int id);
    Task<AbilityValue> GetWithAbilityAsync(int id);
    Task<AbilityValue> SetValueAsync(int id, int newValue);
}
