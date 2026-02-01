using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces;

public interface ISkillService
{
    Task<Skill> CreateAsync(SkillDto dto); 
    Task DeleteAsync(int id); 
    Task<ICollection<Skill>> GetAllAsync();
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
    Task<Skill> GetByIdAsync(int id);
    Task UpdateAsync(int id, SkillDto dto); 
    ICollection<Skill> SortBy(ICollection<Skill> skills, string sortFilter, bool descending = false);
}