using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;
using DndWebApp.Api.Services.Constants;

namespace DndWebApp.Api.Services.Interfaces.Features;

public interface ITraitService : IBaseFeatureService<Trait>
{
    Task<Trait> CreateAsync(TraitDto dto);
    Task DeleteAsync(int traitId);
    Task<ICollection<Trait>> GetAllAsync();
    Task<Trait> GetByIdAsync(int traitId);
    Task UpdateAsync(TraitDto dto, int traitId);
    Task UpdateCollectionsAsync(TraitDto dto, int traitId);
    ICollection<Trait> SortBy(ICollection<Trait> traits, string sortFilter, bool descending = false);
}