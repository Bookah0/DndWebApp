using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces;

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