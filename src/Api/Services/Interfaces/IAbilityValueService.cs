namespace Api.Services.Interfaces;

using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

public interface IAbilityValueService
{
    Task<AbilityValue> CreateAsync(AbilityValueDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<AbilityValue>> GetAllAsync();
    Task<AbilityValue> GetByIdAsync(int id);
    Task<AbilityValue> GetWithAbilityAsync(int id);
    Task<AbilityValue> SetValueAsync(int id, int newValue);
}
