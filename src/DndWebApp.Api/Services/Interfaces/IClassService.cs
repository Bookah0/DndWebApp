using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface IClassService
{
    Task<Class> CreateAsync(ClassDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Class>> GetAllAsync();
    Task<Class> GetByIdAsync(int id);
    Task<Class> GetWithSubclassesAsync(int id);
    Task<Class> GetWithLevelsAsync(int id);
    Task<Class> GetWithFeaturesAsync(int id);
    Task<Class> UpdateAsync(int id, ClassDto dto);
    ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false);
}