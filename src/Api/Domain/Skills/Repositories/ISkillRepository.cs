using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;
using Api.Domain.Skills.DTOs;
using Api.Domain.Skills.Models;

namespace Api.Domain.Skills.Repositories;

public interface ISkillRepository : IRepository<Skill>
{
    Task<Skill> GetByNameAsync(string name);
    Task<Skill> GetWithAbilityAsync(int id);
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
    Task<ICollection<Skill>> GetAllAsync(SkillFilterDto? filter = null, PaginationRequestDto? pagination = null);
}