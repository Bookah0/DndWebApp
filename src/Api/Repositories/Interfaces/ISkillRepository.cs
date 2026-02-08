using Api.Models.Characters;
using Api.Models.DTOs;

namespace Api.Repositories.Interfaces;

public interface ISkillRepository : IRepository<Skill>
{
    Task<Skill> GetByNameAsync(string name);
    Task<Skill> GetWithAbilityAsync(int id);
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
}