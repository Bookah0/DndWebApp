using Api.Domain.Backgrounds.DTOs;
using Api.Domain.Backgrounds.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Backgrounds.Services;

public interface IBackgroundService
{
    Task<Background> CreateAsync(CreateBackgroundRequestDto dto);
    Task DeleteAsync(int id);
	Task<ICollection<Background>> GetAllAsync(BackgroundFilterDto? filter = null, PaginationRequestDto? pagination = null);
    Task<ICollection<Background>> GetAllAsync();
    Task<ICollection<Background>> GetAllWithAllDataAsync();
    Task<Background> GetByIdAsync(int id);
    Task<Background> GetWithFeaturesAsync(int id);
    Task<Background> UpdateAsync(int id, UpdateBackgroundRequestDto dto);
    Task<Background> AddStartingItemsAsync(int id, int itemId);
    Task RemoveStartingItemsAsync(int id, int itemId);
    Task<Background> AddStartingItemChoiceAsync(int id, StartingItemOptionDto dto);
    Task RemoveStartingItemChoiceAsync(int id, int optionId);

}