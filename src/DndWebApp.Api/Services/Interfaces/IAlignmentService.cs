
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Services.Interfaces;

public interface IAlignmentService
{
    Task<Alignment> CreateAsync(AlignmentDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Alignment>> GetAllAsync();
    Task<Alignment> GetByIdAsync(int id);
    Task UpdateAsync(int id, AlignmentDto dto);
    ICollection<Alignment> SortBy(ICollection<Alignment> alignments);

}