using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character; 

namespace DndWebApp.Api.Services.Interfaces;

public interface ISubclassService
{
    Task<Subclass> CreateAsync(ClassDto dto, int parentClassId);
    Task DeleteAsync(int id);
    Task<ICollection<Subclass>> GetAllAsync();
    Task<Subclass> GetByIdAsync(int id);
    Task<Subclass> GetWithLevelsAsync(int id);
    Task<Subclass> GetWithFeaturesAsync(int id);
    Task UpdateAsync(int id, ClassDto dto);
    ICollection<Subclass> SortBy(ICollection<Subclass> classes, bool descending = false);
}