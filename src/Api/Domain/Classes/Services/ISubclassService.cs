using Api.Domain.Classes.Controllers;
using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Classes.Services;

public interface ISubclassService
{
    Task<Subclass> CreateAsync(CreateSubclassRequestDto dto, int parentClassId);
    Task DeleteAsync(int id);
    Task<(int, ICollection<Subclass>)> GetFilteredAsync(SubclassFilterDto filter, PaginationRequestDto pagination);
    Task<Subclass> GetByIdAsync(int id);
    Task<Subclass> GetWithLevelsAsync(int id);
    Task<Subclass> GetWithFeaturesAsync(int id);
    Task<Subclass> UpdateAsync(int id, UpdateSubclassRequestDto dto);
}