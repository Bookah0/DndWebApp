using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IClassLevelRepository : IRepository<ClassLevel>
{
    Task<ClassLevel?> GetWithAllDataAsync(int id);
    Task<ICollection<ClassLevel>> GetAllWithAllDataAsync();
    Task<ClassLevel?> GetWithFeaturesByClassIdAsync(int classId, int level);
}