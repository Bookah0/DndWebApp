using Api.Models.Characters;

namespace Api.Repositories.Interfaces;

public interface ISubclassRepository : IRepository<Subclass>
{
    Task<Subclass> GetWithLevelFeaturesAsync(int id);
    Task<Subclass> GetWithLevelsAsync(int id);
}