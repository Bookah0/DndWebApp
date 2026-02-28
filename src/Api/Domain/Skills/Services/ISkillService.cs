using Api.Domain.Shared.DTOs;
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
    Task<ICollection<Skill>> GetAllAsync(SkillFilterDto? filter = null, PaginationRequestDto? pagination = null);
}