using Api.Models.Characters;
using Api.Models.DTOs.Features;
using Api.Models.Features;

namespace Api.Repositories.Interfaces;

public interface IFeatureRepository<T> : IRepository<T> where T : Feature
{
    Task<T> GetWithProficienciesAsync(int id);
    Task<T> GetWithChoicesAsync(int id);
    Task<T> GetWithAllDataAsync(int id);
}