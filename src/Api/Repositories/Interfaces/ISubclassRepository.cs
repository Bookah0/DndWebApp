using Api.Models.Characters;

namespace Api.Repositories.Interfaces;

public interface ISubclassRepository : IRepository<Subclass>
{
    Task<Subclass> GetWithClassLevelFeaturesAsync(int id);
    Task<Subclass> GetWithClassLevelsAsync(int id);
}