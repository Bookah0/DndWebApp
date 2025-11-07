using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Services.Interfaces;

public interface IAbilityService
{
    Task<Ability> CreateAsync(AbilityDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Ability>> GetAllAsync();
    Task<Ability> GetByIdAsync(int id);
    Task UpdateAsync(AbilityDto dto);
    int GetModifier(AbilityValue val);
    ICollection<Ability> SortBy(ICollection<Ability> abilities);
}