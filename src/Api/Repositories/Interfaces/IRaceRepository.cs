using Api.Models.Characters;
using Api.Models.DTOs;

namespace Api.Repositories.Interfaces;

public interface IRaceRepository : IRepository<Race>
{
    Task<Race> GetWithAllDataAsync(int id);
    Task<Race> GetWithTraitsAsync(int id);
    Task<Race> GetWithSubracesAsync(int id);
}