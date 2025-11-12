using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IAbilityRepository : IRepository<Ability>
{
    Task<Ability?> GetWithSkillsAsync(int id);
    Task<Ability?> GetByNameAsync(string fullName);
    Task<ICollection<Ability>> GetAllWithSkillsAsync();
}