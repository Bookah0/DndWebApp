using Api.Domain.Shared.Repositories;
using Api.Domain.Species.Models;

namespace Api.Domain.Species.Repositories;

public interface ISubraceRepository : IRepository<Subrace>
{
    Task<Subrace> GetWithAllDataAsync(int id);
    Task<Subrace> GetWithTraitsAsync(int id);
    Task<Subrace> GetByNameAsync(string name);
}