using Api.Domain.Classes.Controllers;
using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Classes.Repositories;

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
    Task<ICollection<BaseClass>> GetAllAsync(ClassFilterDto? filter = null, PaginationRequestDto? pagination = null);
}