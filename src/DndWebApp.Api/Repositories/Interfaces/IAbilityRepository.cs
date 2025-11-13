using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IAbilityRepository : IRepository<Ability>
{
    Task<Ability?> GetWithSkillsAsync(int id);
    Task<ICollection<Ability>> GetAllWithSkillsAsync();
    Task<Ability?> GetByTypeAsync(AbilityShortType abilityType);
    Task<Ability?> GetByTypeAsync(AbilityType abilityType);
}