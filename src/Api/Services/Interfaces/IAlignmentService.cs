
using Api.Models.DTOs.RequestDtos.Character;
using Api.Models.World;

namespace Api.Services.Interfaces;

public interface IAlignmentService
{
    Task<Alignment> CreateAsync(AlignmentRequestDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Alignment>> GetAllAsync();
    Task<Alignment> GetByIdAsync(int id);
    Task<Alignment> UpdateAsync(int id, AlignmentRequestDto dto);
    ICollection<Alignment> SortBy(ICollection<Alignment> alignments);

}