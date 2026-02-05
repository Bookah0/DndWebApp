using Api.Data;
using Api.Models.Characters;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented;

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
    public async Task UpdateAsync(Language updatedEntity)
    {
        context.Languages.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

}