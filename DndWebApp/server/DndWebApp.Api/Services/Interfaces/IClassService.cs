using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Services.Interfaces;

public interface IClassService
{
    Task<Class> CreateAsync(ClassDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Class>> GetAllAsync();
    Task<Class> GetByIdAsync(int id);
    Task UpdateAsync(ClassDto dto);
    ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false);
    Task<ClassLevel> AddLevelToClassAsync(ClassLevelDto dto);
    Task UpdateClassLevelAsync(ClassLevelDto dto);
    Task DeleteClassLevelAsync(int id);
    Task<ICollection<ClassLevel>> GetAllLevelsAsync(int classId);
    Task<ClassLevel> GetLevelByIdAsync(int id);
    ICollection<ClassLevel> SortLevels(ICollection<ClassLevel> levels, bool descending = false);
}