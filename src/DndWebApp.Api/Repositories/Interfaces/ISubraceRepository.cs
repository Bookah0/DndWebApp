using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ISubraceRepository : IRepository<Subrace>
{
    Task<Subrace?> GetWithAllDataAsync(int id);
    Task<Subrace?> GetWithTraitsAsync(int id);
    Task<ICollection<Subrace>> GetAllSubracesByRaceAsync(int raceId);
}