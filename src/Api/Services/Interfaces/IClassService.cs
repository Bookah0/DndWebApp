using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;

namespace Api.Services.Interfaces;

public interface IClassService
{
    Task<BaseClass> CreateAsync(CreateClassRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<BaseClass>> GetAllAsync();
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
    ICollection<BaseClass> SortBy(ICollection<BaseClass> classes, bool descending = false);
}