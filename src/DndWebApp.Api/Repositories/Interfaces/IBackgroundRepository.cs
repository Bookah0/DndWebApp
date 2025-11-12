using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IBackgroundRepository : IRepository<Background>
{
    Task<Background?> GetWithFeaturesAsync(int id);
    Task<Background?> GetWithAllDataAsync(int id);
    Task<ICollection<Background>> GetAllWithAllDataAsync();
}