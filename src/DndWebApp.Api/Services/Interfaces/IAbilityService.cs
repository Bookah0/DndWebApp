using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.DTOs.ResponseDtos;

namespace DndWebApp.Api.Services.Interfaces;

public interface IAbilityService
{
    Task<AbilityResponseDto> CreateAsync(AbilityDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<AbilityResponseDto>> GetAllAsync();
    Task<AbilityResponseDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, AbilityDto dto);
    int GetModifier(AbilityValue val);
    ICollection<Ability> SortBy(ICollection<Ability> abilities);
}