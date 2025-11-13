using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.Characters.Enums;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ISkillRepository : IRepository<Skill>
{
    Task<Skill?> GetByTypeAsync(SkillType type);
    Task<Skill?> GetWithAbilityAsync(int id);
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
}