
using Api.Domain.Alignments.DTOs;
using Api.Domain.Alignments.Models;
using Api.Domain.Shared.DTOs;
using Api.Domain.Shared.Utils;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Alignments.Repositories;

public class AlignmentRepository(AppDbContext context) : IAlignmentRepository
{
    public async Task<Alignment> GetByIdAsync(int id) => 
        await context.Alignments.FindAsync(id)
        ?? throw new Exception($"Alignment with id {id} could not be found");

    public async Task<Alignment> GetByNameAsync(string name) => 
        await context.Alignments.FirstOrDefaultAsync(a => a.Name == name)
            ?? throw new Exception($"Alignment with name {name} could not be found");

    public async Task<ICollection<Alignment>> GetAllAsync() => await GetAllAsync(null);
    
    public async Task<Alignment> CreateAsync(Alignment entity)
    {
        await context.Alignments.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Alignment> UpdateAsync(Alignment updatedEntity)
    {
        context.Alignments.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

    public async Task DeleteAsync(Alignment entity)
    {
        context.Alignments.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Alignment>> GetAllAsync(AlignmentFilterDto? filter = null)
    {
        var query = context.Alignments.AsQueryable();

        if(filter is not null)
            query = query.WhereIf(filter.Name, a => a.Name.Contains(filter.Name!));
		
		var alignments = await query.ToListAsync();
        
		var defaultOrder = QueryExtensions.BuildSortOrder(
        [
            "Lawful Good",  "Neutral Good", "Chaotic Good",
            "Lawful Neutral", "Neutral", "Chaotic Neutral",
            "Lawful Evil", "Neutral Evil", "Chaotic Evil"
        ]);
		
		return alignments
			.AsEnumerable()
			.OrderByFixed(a => a.Name, defaultOrder)
			.ToList();
    }
}   