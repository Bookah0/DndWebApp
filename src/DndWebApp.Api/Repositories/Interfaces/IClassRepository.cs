using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IClassRepository : IRepository<Class>
{
    Task<Class> GetWithClassLevelFeaturesAsync(int id);
    Task<Class> GetWithAllDataAsync(int id);
    Task<ICollection<Class>> GetAllWithAllDataAsync();
    Task<Class> GetWithLevelsAsync(int id);
    Task<Class> GetWithStartingEquipmentAsync(int id);
    Task<Class> GetWithSubclassesAsync(int id);
    Task<bool> ExistsAsync(int id);
}