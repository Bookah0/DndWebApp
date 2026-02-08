
using Api.Models.World;

namespace Api.Repositories.Interfaces;

public interface IAlignmentRepository : IRepository<Alignment>
{
    Task<Alignment> GetByNameAsync(string name);
}