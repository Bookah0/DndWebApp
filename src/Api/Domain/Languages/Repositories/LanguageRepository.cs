using Api.Domain.Languages.Models;
using Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Languages.Repositories;

public class LanguageRepository(AppDbContext context) : ILanguageRepository
{
    public async Task<Language> GetByIdAsync(int id) => 
        await context.Languages.FindAsync(id) 
            ?? throw new Exception($"Language with id {id} could not be found");
    
    public async Task<Language> GetByNameAsync(string name) => 
        await context.Languages.FirstOrDefaultAsync(l => l.Name == name)
            ?? throw new Exception($"Language with name {name} could not be found");

    public async Task<ICollection<Language>> GetAllAsync() => await context.Languages.ToListAsync();

    public async Task<Language> CreateAsync(Language entity)
    {
        await context.Languages.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Language entity)
    {
        context.Languages.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task<Language> UpdateAsync(Language updatedEntity)
    {
        context.Languages.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }

}