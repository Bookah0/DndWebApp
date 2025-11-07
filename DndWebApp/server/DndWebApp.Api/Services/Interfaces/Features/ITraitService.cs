using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Enums;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface ITraitService : IBaseFeatureService<Trait>
{
    Task<Trait> CreateAsync(TraitDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Trait>> GetAllAsync();
    Task<Trait> GetByIdAsync(int id);
    Task UpdateAsync(TraitDto dto);
    Task UpdateCollectionsAsync(TraitDto dto);
    ICollection<Trait> SortBy(ICollection<Trait> traits, TraitSortFilter sortFilter, bool descending = false);
}