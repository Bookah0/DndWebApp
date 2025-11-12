using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs.Features;
using DndWebApp.Api.Models.Features;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ITraitRepository : IRepository<Trait>
{
    Task<Trait?> GetWithAllDataAsync(int id);
}