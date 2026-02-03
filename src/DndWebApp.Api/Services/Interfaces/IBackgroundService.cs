using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Interfaces;

public interface IBackgroundService
{
    Task<Background> CreateAsync(BackgroundDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Background>> GetAllAsync();
    Task<Background> GetByIdAsync(int id);
    Task<Background> GetWithFeaturesAsync(int id);
    Task<Background> UpdateAsync(int id, BackgroundDto dto);
    Task<Background> AddStartingItemsAsync(int id, int itemId);
    Task RemoveStartingItemsAsync(int id, int itemId);
    Task<Background> AddStartingItemChoiceAsync(int id, StartingItemOptionDto dto);
    Task RemoveStartingItemChoiceAsync(int id, int optionId);

}