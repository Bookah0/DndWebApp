using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Services.Generic;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services;

public class AlignmentService : IService<Alignment, AlignmentDto, AlignmentDto>
{
    private readonly IRepository<Alignment> repo;
    private readonly ILogger<AlignmentService> logger;

    public AlignmentService(IRepository<Alignment> repo, ILogger<AlignmentService> logger)
    {
        this.repo = repo;
        this.logger = logger;
    }

    public async Task<Alignment> CreateAsync(AlignmentDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.Abbreviation);


        Alignment alignment = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Abbreviation = dto.Abbreviation
        };

        return await repo.CreateAsync(alignment);
    }

    public async Task DeleteClassLevelAsync(int id)
    {
        var alignment = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Alignment could not be found");
        await repo.DeleteAsync(alignment);
    }

    public async Task<ICollection<Alignment>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Alignment> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Alignment could not be found");
    }

    public async Task UpdateAsync(AlignmentDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.Abbreviation);

        var alignment = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Alignment could not be found");

        alignment.Name = dto.Name;
        alignment.Description = dto.Description;
        alignment.Abbreviation = dto.Abbreviation;

        await repo.UpdateAsync(alignment);
    }

    public ICollection<Alignment> SortBy(ICollection<Alignment> alignments)
    {
        string[] fixedSortOrder =
        [
            "Lawful Good",  "Neutral Good", "Chaotic Good",
            "Lawful Neutral", "True Neutral", "Chaotic Neutral",
            "Lawful Evil", "Neutral Evil", "Chaotic Evil"
        ];

        var alignmentOrder = SortUtil.CreateOrderLookup(fixedSortOrder);

        return [.. alignments.OrderBy(a => alignmentOrder[a.Name])];
    }
}