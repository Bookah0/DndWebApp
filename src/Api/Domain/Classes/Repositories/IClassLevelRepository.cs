using Api.Domain.Classes.Models;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Classes.Repositories;

public interface IClassLevelRepository : IRepository<ClassLevel>
{
    Task<ClassLevel> GetWithAllDataAsync(int id);
    Task<ICollection<ClassLevel>> GetAllWithAllDataAsync();
    Task<ClassLevel> GetWithFeaturesByClassIdAsync(int classId, int level);
}