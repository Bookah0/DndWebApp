using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Models;

namespace Api.Domain.Shared.Repositories;

public interface IFeatureRepository<T, FF> : IRepository<T> where T : Feature where FF : FeatureFilterDto
{
    Task<T> GetWithProficienciesAsync(int id);
    Task<T> GetWithChoicesAsync(int id);
    Task<T> GetWithAllDataAsync(int id);
	Task<ICollection<T>> GetAllAsync(FF? filter = null, PaginationRequestDto? pagination = null);
}