using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IClassFeatureRepository : IRepository<ClassFeature>
{
    Task<ClassFeature?> GetWithAllDataAsync(int id);
    Task<ICollection<ClassFeature>> GetAllWithAllDataAsync();
}