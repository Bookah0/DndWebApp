using Api.Domain.Shared.Models;

namespace Api.Domain.Shared.Repositories;

public interface IFeatureRepository<T> : IRepository<T> where T : Feature
{
    Task<T> GetWithProficienciesAsync(int id);
    Task<T> GetWithChoicesAsync(int id);
    Task<T> GetWithAllDataAsync(int id);
}