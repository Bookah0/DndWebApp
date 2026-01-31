using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.DTOs.ResponseDtos;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface ITraitService : IBaseFeatureService<Trait>
{
    Task<TraitResponseDto> CreateAsync(TraitDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<TraitResponseDto>> GetAllAsync();
    Task<TraitResponseDto> GetByIdAsync(int id);
    Task UpdateAsync(int id, TraitDto dto);
    Task UpdateCollectionsAsync(int id, TraitDto dto);
    ICollection<Trait> SortBy(ICollection<Trait> traits, TraitSortFilter sortFilter, bool descending = false);
}