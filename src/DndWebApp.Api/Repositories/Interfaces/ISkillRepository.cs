using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ISkillRepository : IRepository<Skill>
{
    Task<Skill?> GetByNameAsync(string name);
    Task<Skill?> GetWithAbilityAsync(int id);
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
}