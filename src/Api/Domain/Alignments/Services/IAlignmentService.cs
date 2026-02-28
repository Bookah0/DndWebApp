using Api.Domain.Alignments.DTOs;
using Api.Domain.Alignments.Models;

namespace Api.Domain.Alignments.Services;

public interface IAlignmentService
{
    Task<Alignment> CreateAsync(AlignmentRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Alignment>> GetAllAsync();
    Task<ICollection<Alignment>> GetAllAsync(AlignmentFilterDto? filter = null);
    Task<Alignment> GetByIdAsync(int id);
    Task<Alignment> UpdateAsync(int id, AlignmentRequestDto dto);

}