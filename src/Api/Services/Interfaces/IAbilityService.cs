using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces;

public interface IAbilityService
{
    Task<Ability> CreateAsync(AbilityDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Ability>> GetAllAsync();
    Task<Ability> GetByIdAsync(int id);
    Task<Ability> UpdateAsync(int id, AbilityDto dto);
    int GetModifier(AbilityValue val);
    ICollection<Ability> SortBy(ICollection<Ability> abilities);
}