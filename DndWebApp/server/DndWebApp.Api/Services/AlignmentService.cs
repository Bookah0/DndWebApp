using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Services.Util;
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
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.Abbreviation);

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
        ValidationUtil.ValidateRequiredString(dto.Name);
        ValidationUtil.ValidateRequiredString(dto.Description);
        ValidationUtil.ValidateRequiredString(dto.Abbreviation);

        var alignment = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Alignment could not be found");
        
        alignment.Name = dto.Name;
        alignment.Description = dto.Description;
        alignment.Abbreviation = dto.Abbreviation;

        await repo.UpdateAsync(alignment);
        await context.SaveChangesAsync();
    }
}