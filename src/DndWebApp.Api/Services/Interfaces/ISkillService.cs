using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface ISkillService
{
    Task<Skill> CreateAsync(SkillDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Skill>> GetAllAsync();
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
    Task<Skill> GetByIdAsync(int id);
    Task<Skill> UpdateAsync(int id, SkillDto dto); 
    ICollection<Skill> SortBy(ICollection<Skill> skills, string sortFilter, bool descending = false);
}