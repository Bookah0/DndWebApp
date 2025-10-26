using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
namespace DndWebApp.Api.Services;

public class AlignmentService : IService<Alignment, AlignmentDto, AlignmentDto>
{
    protected IRepository<Alignment> repo;
    protected AppDbContext context;

    public AlignmentService(IRepository<Alignment> repo, AppDbContext context)
    {
        this.context = context;
        this.repo = repo;
    }

    public async Task<Alignment> CreateAsync(AlignmentDto dto)
    {
        if (dto is null)
            throw new NullReferenceException("Dto can't be null");
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException($"Name cannot be null, empty, or whitespace.");
        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException($"Description cannot be null, empty, or whitespace.");
        if (string.IsNullOrWhiteSpace(dto.Abbreviation))
            throw new ArgumentException($"Abbreviation cannot be null, empty, or whitespace.");

        Alignment alignment = new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Abbreviation = dto.Abbreviation
        };
        
        await repo.CreateAsync(alignment);
        await context.SaveChangesAsync();
        return alignment;
    }

    public async Task DeleteAsync(int id)
    {
        var alignment = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Alignment could not be found");
        await repo.DeleteAsync(alignment);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Alignment>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Alignment> GetByIdAsync(int id)
    {
        var alignment = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Alignment could not be found");
        return alignment;
    }

    public async Task UpdateAsync(AlignmentDto dto)
    {
        if (dto is null)
            throw new NullReferenceException("Dto can't be null");
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException($"Name cannot be null, empty, or whitespace.");
        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException($"Description cannot be null, empty, or whitespace.");
        if (string.IsNullOrWhiteSpace(dto.Abbreviation))
            throw new ArgumentException($"Abbreviation cannot be null, empty, or whitespace.");

        var alignment = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Alignment could not be found");
        
        alignment.Name = dto.Name;
        alignment.Description = dto.Description;
        alignment.Abbreviation = dto.Abbreviation;

        await repo.UpdateAsync(alignment);
        await context.SaveChangesAsync();
    }
}