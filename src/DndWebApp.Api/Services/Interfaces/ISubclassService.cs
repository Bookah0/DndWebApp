using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Services.Interfaces;

public interface ISubclassService
{
    Task<Subclass> CreateAsync(ClassDto dto, int parentClassId);
    Task DeleteAsync(int id);
    Task<ICollection<Subclass>> GetAllAsync();
    Task<Subclass> GetByIdAsync(int id);
    Task UpdateAsync(ClassDto dto, int? newParentClassId);
    ICollection<Class> SortBy(ICollection<Class> classes, bool descending = false);
}