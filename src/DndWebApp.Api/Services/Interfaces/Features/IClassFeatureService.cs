using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IClassFeatureService : IBaseFeatureService<ClassFeature>
{
    Task<ClassFeature> CreateAsync(ClassFeatureDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<ClassFeature>> GetAllAsync();
    Task<ClassFeature> GetByIdAsync(int id);
    Task UpdateAsync(ClassFeatureDto dto);
    ICollection<ClassFeature> SortBy(ICollection<ClassFeature> features, ClassFeatureSortFilter sortFilter, bool descending = false);
}