using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Spells;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented.Classes;

public class ClassRepository : IClassRepository
{
    private readonly AppDbContext context;

    public ClassRepository(AppDbContext context, IRepository<Class> baseRepo)
    {
        this.context = context;
    }

    public async Task<Class?> GetWithAllDataAsync(int id)
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.OptionIds)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithClassLevelsAsync(int id)
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithClassLevelFeaturesAsync(int id)
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
                .ThenInclude(l => l.NewFeatures)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Class?> GetWithStartingEquipmentAsync(int id)
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.OptionIds)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Class>> GetAllWithAllDataAsync()
    {
        return await context.Classes
            .AsSplitQuery()
            .Include(b => b.ClassLevels)
            .Include(b => b.StartingEquipment)
            .Include(b => b.StartingEquipmentOptions)
                .ThenInclude(o => o.OptionIds)
            .ToListAsync();
    }

    public Task<ICollection<Class>> FilterAllAsync(SpellFilter spellFilter)
    {
        throw new NotImplementedException();
    }
}