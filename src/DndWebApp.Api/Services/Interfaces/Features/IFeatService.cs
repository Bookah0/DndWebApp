using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Implemented.Features;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface IFeatService : IBaseFeatureService<Feat>
{

    Task<FeatResponseDto> CreateAsync(FeatDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<FeatResponseDto>> GetAllAsync();
    Task<FeatResponseDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, FeatDto dto);
    Task UpdateCollectionsAsync(int id, FeatDto dto);
    ICollection<Feat> SortBy(ICollection<Feat> feats, bool descending = false);
}