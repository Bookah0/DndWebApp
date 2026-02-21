using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Models;

namespace Api.Domain.Classes.Services;

public interface ISubclassService
{
    Task<Subclass> CreateAsync(CreateSubclassRequestDto dto, int parentClassId);
    Task DeleteAsync(int id);
    Task<ICollection<Subclass>> GetAllAsync();
    Task<Subclass> GetByIdAsync(int id);
    Task<Subclass> GetWithLevelsAsync(int id);
    Task<Subclass> GetWithFeaturesAsync(int id);
    Task<Subclass> UpdateAsync(int id, UpdateSubclassRequestDto dto);
    ICollection<Subclass> SortBy(ICollection<Subclass> classes, bool descending = false);
}