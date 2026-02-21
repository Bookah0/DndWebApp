using Api.Domain.Species.DTOs;
using Api.Domain.Species.Models;

namespace Api.Domain.Species.Services;

public interface IRaceService
{
    Task<Race> CreateAsync(CreateRaceRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Race>> GetAllAsync();
    Task<Race> GetByIdAsync(int id);
    Task<Race> GetWithSubracesAsync(int id);
    Task<Race> GetWithTraitsAsync(int id);
    Task<Race> GetWithAllDataAsync(int id);
    Task<Race> UpdateAsync(int id, UpdateRaceRequestDto dto);
}