using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.Features;

namespace Api.Services.Interfaces;

public interface IClassLevelService
{
    Task<ClassLevel> CreateAsync(CreateClassLevelRequestDto dto);
    Task<ClassLevel> UpdateAsync(int id, UpdateClassLevelRequestDto dto);
    Task DeleteAsync(int id);
    Task<ClassLevel> GetByIdAsync(int id);
    
    Task<ClassLevel> AddFeatureAsync(int levelId, int featureId);
    Task<ClassLevel> AddFeatureAsync(ClassLevel level, ClassFeature feature);
    Task<ClassLevel> RemoveFeatureAsync(int levelId, int featureId);
    Task<ClassLevel> RemoveFeatureAsync(ClassLevel level, ClassFeature feature);

    Task<ClassLevel> AddClassSlotAsync(int levelId, ClassSlotRequestDto slot);
    Task<ClassLevel> RemoveClassSlotByNameAsync(int levelId, string slotName);

    ICollection<ClassLevel> SortByLevel(ICollection<ClassLevel> levels, bool descending = false);
}