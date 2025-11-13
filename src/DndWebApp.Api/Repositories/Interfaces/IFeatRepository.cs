using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IFeatRepository : IRepository<Feat>
{
    Task<Feat?> GetWithAllDataAsync(int id);
    Task<ICollection<Feat>> GetAllWithAllDataAsync();
    Task<Feat?> GetByNameAsync(string name);
}