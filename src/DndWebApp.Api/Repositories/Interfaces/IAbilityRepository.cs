using DndWebApp.Api.Models.Characters;
namespace DndWebApp.Api.Repositories.Interfaces;

public interface IAbilityRepository : IRepository<Ability>
{
    Task<Ability> GetWithSkillsAsync(int id);
    Task<ICollection<Ability>> GetAllWithSkillsAsync();
    Task<Ability> GetByShortNameAsync(string name);
}