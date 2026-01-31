
using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.DTOs.Character;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Interfaces;
using static DndWebApp.Api.Services.Util.SortUtil;

namespace DndWebApp.Api.Services.Implemented;

public class AlignmentService : IAlignmentService
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
        Alignment alignment = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Abbreviation = dto.Abbreviation
        };

        return await repo.CreateAsync(alignment);
    }

    public async Task DeleteAsync(int id)
    {
        var alignment = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Alignment could not be found");
        await repo.DeleteAsync(alignment);
    }

    public async Task<ICollection<Alignment>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Alignment> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id) ?? throw new NotFoundException("Alignment could not be found");
    }

    public async Task UpdateAsync(int id, AlignmentDto dto)
    {
        var alignment = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Alignment could not be found");

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

        var alignmentOrder = CreateOrderLookup(fixedSortOrder);

        return [.. alignments.OrderBy(a => alignmentOrder[a.Name])];
    }
}