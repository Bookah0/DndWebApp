using DndWebApp.Api.Data;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class AlignmentRepository(AppDbContext context) : IAlignmentRepository
{
    public async Task<Alignment> GetByIdAsync(int id) => 
        await context.Alignments.FindAsync(id)
        ?? throw new Exception($"Alignment with id {id} could not be found");

    public async Task<Alignment> GetByNameAsync(string name) => 
        await context.Alignments.FirstOrDefaultAsync(a => a.Name == name)
            ?? throw new Exception($"Alignment with name {name} could not be found");

    public async Task<ICollection<Alignment>> GetAllAsync() => await context.Alignments.ToListAsync();
    
    public async Task<Alignment> CreateAsync(Alignment entity)
    {
        await context.Alignments.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Alignment updatedEntity)
    {
        context.Alignments.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Alignment entity)
    {
        context.Alignments.Remove(entity);
        await context.SaveChangesAsync();
    }
}   