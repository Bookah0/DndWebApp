using Api.Domain.Shared.Repositories;
using Api.Domain.Species.Models;

namespace Api.Domain.Species.Repositories;

public interface IRaceRepository : IRepository<Race>
{
    Task<Race> GetWithAllDataAsync(int id);
    Task<Race> GetWithTraitsAsync(int id);
    Task<Race> GetWithSubracesAsync(int id);
}