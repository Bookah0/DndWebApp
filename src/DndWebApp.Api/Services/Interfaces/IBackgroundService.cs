using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface IBackgroundService
{
    Task<Background> CreateAsync(BackgroundDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Background>> GetAllAsync();
    Task<Background> GetByIdAsync(int id);
    Task<Background> GetWithFeaturesAsync(int id);
    Task UpdateAsync(int id, BackgroundDto dto);
    ICollection<Background> SortBy(ICollection<Background> backgrounds, bool descending = false);
}