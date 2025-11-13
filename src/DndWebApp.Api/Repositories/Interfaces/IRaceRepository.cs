using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IRaceRepository : IRepository<Race>
{
    Task<Race?> GetByNameAsync(string name);
    Task<Race?> GetWithAllDataAsync(int id);
    Task<Race?> GetWithTraitsAsync(int id);
}