using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;

namespace DndWebApp.Api.Services.Interfaces;

public interface IAlignmentService
{
    Task<Alignment> CreateAsync(AlignmentDto dto);
    Task DeleteAsync(int id);
    Task<ICollection<Alignment>> GetAllAsync();
    Task<Alignment> GetByIdAsync(int id);
    Task UpdateAsync(AlignmentDto dto);
    ICollection<Alignment> SortBy(ICollection<Alignment> alignments);

}