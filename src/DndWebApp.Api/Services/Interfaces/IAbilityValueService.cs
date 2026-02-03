namespace DndWebApp.Api.Services.Interfaces;

using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

public interface IAbilityValueService
{
    Task<AbilityValue> CreateAsync(AbilityValueDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<AbilityValue>> GetAllAsync();
    Task<AbilityValue> GetByIdAsync(int id);
    Task<AbilityValue> GetWithAbilityAsync(int id);
    Task SetValueAsync(int id, int newValue);
}
