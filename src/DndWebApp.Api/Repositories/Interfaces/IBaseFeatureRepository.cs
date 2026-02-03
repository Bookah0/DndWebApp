using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IFeatureRepository<T> : IRepository<T> where T : AFeature
{
    Task<T> GetWithProficienciesAsync(int id);
    Task<T> GetWithChoicesAsync(int id);
    Task<T> GetWithAllDataAsync(int id);
}