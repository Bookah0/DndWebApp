using Api.Domain.Classes.DTOs;
using Api.Domain.Classes.Models;
using Api.Domain.Shared.DTOs;

namespace Api.Domain.Classes.Services;

public interface IBaseClassService
{
    Task<BaseClass> CreateAsync(CreateClassRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<BaseClass>> GetAllAsync();
    Task<(int, ICollection<BaseClass>)> GetFilteredAsync(ClassFilterDto filter, PaginationRequestDto pagination);
    Task<BaseClass> GetByIdAsync(int id);
    Task<BaseClass> GetWithSubclassesAsync(int id);
    Task<BaseClass> GetWithLevelsAsync(int id);
    Task<BaseClass> GetWithFeaturesAsync(int id);
    Task<BaseClass> UpdateAsync(int id, UpdateClassRequestDto dto);
    Task<BaseClass> AddClassLevel(int id, int classLevelId);
    Task<BaseClass> RemoveClassLevel(int id, int classLevelId);
    Task<BaseClass> AddStartingEquipment(int id, int equipmentId);
    Task<BaseClass> RemoveStartingEquipment(int id, int equipmentId);
    // Task<BaseClass> AddStartingEquipmentChoice(int id, int choiceId);
    // Task<BaseClass> RemoveStartingEquipmentChoice(int id, int choiceId);
}