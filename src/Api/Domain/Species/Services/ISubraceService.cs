using Api.Domain.Species.DTOs;
using Api.Domain.Species.Models;

namespace Api.Domain.Species.Services;

public interface ISubraceService
{
    Task<Subrace> CreateAsync(CreateSubraceRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Subrace>> GetAllAsync();
    Task<Subrace> GetByIdAsync(int id);
    Task<Subrace> GetWithTraitsAsync(int id);
    Task<Subrace> GetWithAllDataAsync(int id);
    Task<Subrace> UpdateAsync(int id, UpdateSubraceRequestDto dto);
}