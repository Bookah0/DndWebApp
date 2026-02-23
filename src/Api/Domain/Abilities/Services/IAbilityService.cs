using Api.Domain.Abilities.DTOs;
using Api.Domain.Abilities.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Abilities.Services;

public interface IAbilityService
{
    Task<Ability> CreateAsync(AbilityRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Ability>> GetAllAsync();
    Task<Ability> GetByIdAsync(int id);
    Task<Ability> UpdateAsync(int id, AbilityRequestDto dto);
    int GetModifier(AbilityValue val);
    Task<(int, ICollection<Ability>)> GetFilteredAsync(string? nameFilter, PaginationRequestDto pagination);
}