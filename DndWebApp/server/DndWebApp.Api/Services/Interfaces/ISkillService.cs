using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces;

public interface ISkillService
{
    Task<Skill> CreateAsync(SkillDto dto); 
    Task DeleteAsync(int id); Task<ICollection<Skill>> GetAllAsync();
    Task<ICollection<Skill>> GetAllWithAbilityAsync();
    Task<Skill> GetByIdAsync(int id);
    Task UpdateAsync(SkillDto dto); 
    ICollection<Skill> SortBy(ICollection<Skill> skills, SkillSortFilter SkillSortFilter, bool descending = false);
}