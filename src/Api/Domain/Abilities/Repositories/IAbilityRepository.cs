using Api.Domain.Abilities.DTOs;
using Api.Domain.Abilities.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Abilities.Repositories;

public interface IAbilityRepository : IRepository<Ability>
{
    Task<Ability> GetWithSkillsAsync(int id);
    Task<ICollection<Ability>> GetAllWithSkillsAsync();
    Task<Ability> GetByShortNameAsync(string name);
    Task<ICollection<Ability>> GetAllAsync(AbilityFilterDto? filter = null);
}