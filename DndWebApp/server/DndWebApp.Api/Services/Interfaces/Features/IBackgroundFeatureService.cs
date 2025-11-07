
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IBackgroundFeatureService : IBaseFeatureService<BackgroundFeature>
{
    Task<BackgroundFeature> CreateAsync(BackgroundFeatureDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<BackgroundFeature>> GetAllAsync();
    Task<BackgroundFeature> GetByIdAsync(int id);
    Task UpdateAsync(BackgroundFeatureDto dto);
    ICollection<BackgroundFeature> SortBy(ICollection<BackgroundFeature> features, BackgroundFeatureSortFilter sortFilter, bool descending = false);
}