using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IClassRepository : IRepository<Class>
{
    Task<Class?> GetWithClassLevelFeaturesAsync(int id);
    Task<Class?> GetWithAllDataAsync(int id);
    Task<ICollection<Class>> GetAllWithAllDataAsync();
    Task<Class?> GetWithClassLevelsAsync(int id);
    Task<Class?> GetByTypeAsync(ClassType type);
}