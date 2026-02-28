using Api.Domain.Alignments.DTOs;
using Api.Domain.Alignments.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Repositories;

namespace Api.Domain.Alignments.Repositories;

public interface IAlignmentRepository : IRepository<Alignment>
{
    Task<Alignment> GetByNameAsync(string name);
    Task<ICollection<Alignment>> GetAllAsync(AlignmentFilterDto? filter = null);
}