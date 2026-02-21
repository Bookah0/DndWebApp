using Api.Domain.Shared.Repositories;
using Api.Domain.Skills.Models;

namespace Api.Domain.Skills.Repositories;

public interface ISkillRepository : IRepository<Skill>
{
    Task<Skill> GetByNameAsync(string name);
    Task<Skill> GetWithAbilityAsync(int id);
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
}