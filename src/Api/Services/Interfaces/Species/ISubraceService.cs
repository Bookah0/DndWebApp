using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces.Species;

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