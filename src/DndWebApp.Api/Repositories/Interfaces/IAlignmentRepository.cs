
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Repositories.Interfaces;

public interface IAlignmentRepository : IRepository<Alignment>
{
    Task<Alignment?> GetByNameAsync(string name);
}