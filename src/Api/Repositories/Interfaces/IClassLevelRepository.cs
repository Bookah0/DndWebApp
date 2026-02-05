using Api.Models.Characters;
using Api.Models.DTOs;

namespace Api.Repositories.Interfaces;

public interface IClassLevelRepository : IRepository<ClassLevel>
{
    Task<ClassLevel> GetWithAllDataAsync(int id);
    Task<ICollection<ClassLevel>> GetAllWithAllDataAsync();
    Task<ClassLevel> GetWithFeaturesByClassIdAsync(int classId, int level);
}