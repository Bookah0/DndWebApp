using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class LanguageRepository : ILanguageRepository
{
    private readonly AppDbContext context;

    public LanguageRepository(AppDbContext context)
    {
        this.context = context;
    }

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

    public async Task<ICollection<Language>> GetMiscellaneousItemsAsync() => await context.Languages.ToListAsync();
    public async Task<Language?> GetByIdAsync(int id) => await context.Languages.FindAsync(id);
    public async Task<Language?> GetByNameAsync(string name) => await context.Languages.FirstOrDefaultAsync(l => l.Name == name);
}