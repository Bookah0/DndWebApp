using Api.Domain.Skills.DTOs;
using Api.Domain.Skills.Models;

namespace Api.Domain.Skills.Services;

public interface ISkillService
{
    Task<Skill> CreateAsync(CreateSkillRequestDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Skill>> GetAllAsync();
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
    Task<Skill> GetByIdAsync(int id);
    Task<Skill> UpdateAsync(int id, UpdateSkillRequestDto dto); 
    ICollection<Skill> SortBy(ICollection<Skill> skills, string sortFilter, bool descending = false);
}