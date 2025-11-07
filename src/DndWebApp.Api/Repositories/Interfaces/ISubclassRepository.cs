using DndWebApp.Api.Models.Characters;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface ISubclassRepository : IRepository<Subclass>
{
    Task<Subclass?> GetWithClassLevelFeaturesAsync(int id);
    Task<Subclass?> GetWithClassLevelsAsync(int id);
}