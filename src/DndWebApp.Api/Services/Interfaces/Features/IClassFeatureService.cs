using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IClassFeatureService : IBaseFeatureService<ClassFeature>
{
    Task<ClassFeatureResponseDto> CreateAsync(ClassFeatureDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<ClassFeatureResponseDto>> GetAllAsync();
    Task<ClassFeatureResponseDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, ClassFeatureDto dto);
    ICollection<ClassFeature> SortBy(ICollection<ClassFeature> features, string sortFilter, bool descending = false);
}