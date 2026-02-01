using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface IClassLevelService
{
    Task<ClassLevel> CreateAsync(ClassLevelDto dto);
    Task UpdateAsync(int id, ClassLevelDto dto);
    Task DeleteAsync(int id);
    Task<ClassLevel> GetByIdAsync(int id);
    ICollection<ClassLevel> SortByLevel(ICollection<ClassLevel> levels, bool descending = false);
}