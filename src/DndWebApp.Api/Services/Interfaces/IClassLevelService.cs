using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface IClassLevelService
{
    Task<ClassLevel> AddLevelToClassAsync(ClassLevelDto dto);
    Task UpdateClassLevelAsync(int id, ClassLevelDto dto);
    Task DeleteClassLevelAsync(int id);
    Task<ClassLevel> GetLevelByIdAsync(int id);
    ICollection<ClassLevel> SortByLevel(ICollection<ClassLevel> levels, bool descending = false);
}