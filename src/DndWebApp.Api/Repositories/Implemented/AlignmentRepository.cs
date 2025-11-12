using DndWebApp.Api.Data;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class AlignmentRepository : IAlignmentRepository
{
    private readonly AppDbContext context;

    public AlignmentRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Alignment> CreateAsync(Alignment entity)
    {
        await context.Alignments.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Alignment entity)
    {
        context.Alignments.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Alignment>> GetMiscellaneousItemsAsync() => await context.Alignments.ToListAsync();
    public async Task<Alignment?> GetByIdAsync(int id) => await context.Alignments.FindAsync(id);
    public async Task<Alignment?> GetByNameAsync(string fullName) => await context.Alignments.FirstOrDefaultAsync(x => x.Name == fullName);

    public async Task UpdateAsync(Alignment updatedEntity)
    {
        context.Alignments.Update(updatedEntity);
        await context.SaveChangesAsync();
    }
}   