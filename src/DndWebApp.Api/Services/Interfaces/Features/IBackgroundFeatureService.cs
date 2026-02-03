
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IBackgroundFeatureService : IBaseFeatureService<BackgroundFeature>
{
    Task<BackgroundFeature> CreateAsync(BackgroundFeatureDto dto);
    Task UpdateAsync(BackgroundFeatureDto dto, int id);
    ICollection<BackgroundFeature> SortBy(ICollection<BackgroundFeature> features, string sortFilter, bool descending = false);
}