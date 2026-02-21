using Api.Domain.Alignments.Models;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Alignments.Repositories;

public interface IAlignmentRepository : IRepository<Alignment>
{
    Task<Alignment> GetByNameAsync(string name);
}