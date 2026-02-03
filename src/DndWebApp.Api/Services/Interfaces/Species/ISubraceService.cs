using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Interfaces.Species;

public interface ISubraceService
{
    Task<Subrace> CreateAsync(SubraceDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Subrace>> GetAllAsync();
    Task<Subrace> GetByIdAsync(int id);
    Task<Subrace> GetWithTraitsAsync(int id);
    Task<Subrace> GetWithAllDataAsync(int id);
    Task<Subrace> UpdateAsync(int id, SubraceDto dto);
}