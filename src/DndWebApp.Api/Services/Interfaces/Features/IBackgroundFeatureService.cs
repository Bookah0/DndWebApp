
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IBackgroundFeatureService : IBaseFeatureService<BackgroundFeature>
{
    Task<BackgroundFeatureResponseDto> CreateAsync(BackgroundFeatureDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<BackgroundFeatureResponseDto>> GetAllAsync();
    Task<BackgroundFeatureResponseDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, BackgroundFeatureDto dto);
    ICollection<BackgroundFeature> SortBy(ICollection<BackgroundFeature> features, string sortFilter, bool descending = false);
}