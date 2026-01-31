using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IClassFeatureService : IBaseFeatureService<ClassFeature>
{
    Task<ClassFeatureResponseDto> CreateAsync(ClassFeatureDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<ClassFeatureResponseDto>> GetAllAsync();
    Task<ClassFeatureResponseDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, ClassFeatureDto dto);
    ICollection<ClassFeature> SortBy(ICollection<ClassFeature> features, ClassFeatureSortFilter sortFilter, bool descending = false);
}