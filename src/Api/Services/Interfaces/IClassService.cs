using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces;

public interface IClassService
{
    Task<BaseClass> CreateAsync(ClassDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<BaseClass>> GetAllAsync();
    Task<BaseClass> GetByIdAsync(int id);
    Task<BaseClass> GetWithSubclassesAsync(int id);
    Task<BaseClass> GetWithLevelsAsync(int id);
    Task<BaseClass> GetWithFeaturesAsync(int id);
    Task<BaseClass> UpdateAsync(int id, ClassDto dto);
    ICollection<BaseClass> SortBy(ICollection<BaseClass> classes, bool descending = false);
}