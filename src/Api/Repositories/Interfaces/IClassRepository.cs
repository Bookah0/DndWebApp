using Api.Models.Characters;

namespace Api.Repositories.Interfaces;

public interface IBaseClassRepository : IRepository<BaseClass>
{
    Task<BaseClass> GetWithLevelFeaturesAsync(int id);
    Task<BaseClass> GetWithAllDataAsync(int id);
    Task<ICollection<BaseClass>> GetAllWithAllDataAsync();
    Task<ICollection<BaseClass>> GetAllWithLevelFeaturesAsync();
    Task<BaseClass> GetWithLevelsAsync(int id);
    Task<BaseClass> GetWithStartingEquipmentAsync(int id);
    Task<BaseClass> GetWithSubclassesAsync(int id);
    Task<bool> ExistsAsync(int id);
}