using Api.Domain.Classes.Models;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Classes.Repositories;

public interface ISubclassRepository : IRepository<Subclass>
{
    Task<Subclass> GetWithLevelFeaturesAsync(int id);
    Task<Subclass> GetWithLevelsAsync(int id);
}