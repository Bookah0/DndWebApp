

using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IBackgroundFeatureRepository : IRepository<BackgroundFeature>
{
    Task<BackgroundFeature?> GetWithAllDataAsync(int id);
    Task<ICollection<BackgroundFeature>> GetAllWithAllDataAsync();
}