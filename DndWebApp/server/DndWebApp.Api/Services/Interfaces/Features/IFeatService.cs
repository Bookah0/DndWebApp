using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Implemented.Features;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IFeatService : IBaseFeatureService<Feat>
{

    Task<Feat> CreateAsync(FeatDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Feat>> GetAllAsync();
    Task<Feat> GetByIdAsync(int id);
    Task UpdateAsync(FeatDto dto);
    Task UpdateCollectionsAsync(FeatDto dto);
    ICollection<Feat> SortBy(ICollection<Feat> feats, bool descending = false);
}