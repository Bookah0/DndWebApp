using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Interfaces.Species;

public interface IRaceService
{
    Task<Race> CreateAsync(RaceDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Race>> GetAllAsync();
    Task<Race> GetByIdAsync(int id);
    Task<Race> GetWithSubracesAsync(int id);
    Task<Race> GetWithTraitsAsync(int id);
    Task<Race> GetWithAllDataAsync(int id);
    Task<Race> UpdateAsync(int id, RaceDto dto);
}