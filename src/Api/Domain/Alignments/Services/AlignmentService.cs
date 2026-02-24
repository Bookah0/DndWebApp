using Api.Domain.Alignments.DTOs;
using Api.Domain.Alignments.Models;
using Api.Domain.Shared.Repositories;
using Api.Domain.Shared.Utils;

namespace Api.Domain.Alignments.Services;

public class AlignmentService(IRepository<Alignment> repo, ILogger<AlignmentService> logger) : IAlignmentService
{
    public async Task<Alignment> CreateAsync(AlignmentRequestDto dto)
    {
        logger.LogInformation("Creating alignment, Name: {AlignmentName}", dto.Name);
        var alignment = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Abbreviation = dto.Abbreviation
        });
        
        logger.LogInformation("Successfully created alignment, Name: {AlignmentName}, ID: {AlignmentId}", alignment.Name, alignment.Id);
        return alignment;
    }

    public async Task DeleteAsync(int id)
    {
        var alignment = await repo.GetByIdAsync(id);
        logger.LogInformation("Deleting alignment, Name: {AlignmentName}, ID: {AlignmentId}", alignment.Name, id);
        await repo.DeleteAsync(alignment);
        logger.LogInformation("Successfully deleted alignment, Name: {AlignmentName}, ID: {AlignmentId}", alignment.Name, id);
    }

    public async Task<ICollection<Alignment>> GetAllAsync() => await repo.GetAllAsync();
    public async Task<Alignment> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
    
    public async Task<Alignment> UpdateAsync(int id, AlignmentRequestDto dto)
    {
        var alignment = await repo.GetByIdAsync(id);

        logger.LogInformation("Updating alignment, Name: {AlignmentName}, ID: {AlignmentId}", alignment.Name, id);

        alignment.Name = dto.Name;
        alignment.Description = dto.Description;
        alignment.Abbreviation = dto.Abbreviation;

        await repo.UpdateAsync(alignment);
        logger.LogInformation("Successfully updated alignment, Name: {AlignmentName}, ID: {AlignmentId}", alignment.Name, id);
        return alignment;
    }

    // TODO Move to database level sorting
    public ICollection<Alignment> SortBy(ICollection<Alignment> alignments)
    {
        string[] fixedSortOrder =
        [
            "Lawful Good",  "Neutral Good", "Chaotic Good",
            "Lawful Neutral", "True Neutral", "Chaotic Neutral",
            "Lawful Evil", "Neutral Evil", "Chaotic Evil"
        ];

        var alignmentOrder = FilterUtils.CreateOrderLookup(fixedSortOrder);

        return [.. alignments.OrderBy(a => alignmentOrder[a.Name])];
    }
}