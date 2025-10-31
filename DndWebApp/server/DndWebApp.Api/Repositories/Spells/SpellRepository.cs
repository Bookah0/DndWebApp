using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Spells;

public class SpellRepository : ISpellRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Spell> baseRepo;

    public SpellRepository(AppDbContext context, IRepository<Spell> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Spell> CreateAsync(Spell entity) => await baseRepo.CreateAsync(entity);
    public async Task<Spell?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Spell>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Spell updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Spell entity) => await baseRepo.DeleteAsync(entity);   

    public async Task<Spell?> GetWithClassesAsync(int id)
    {
        return await context.Spells
            .Include(s => s.Classes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Spell>> GetAllWithClassesAsync()
    {
        return await context.Spells
            .Include(s => s.Classes)
            .ToListAsync();
    }
    
    public async Task<ICollection<Spell>> FilterAllAsync(SpellFilter filter)
    {
        var query = context.Spells.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(s => s.Name.Contains(filter.Name));

        if (filter.MinLevel.HasValue)
            query = query.Where(s => s.Level >= filter.MinLevel.Value);

        if (filter.MaxLevel.HasValue)
            query = query.Where(s => s.Level <= filter.MaxLevel.Value);

        if (filter.MagicSchools is not null && filter.MagicSchools.Count != 0)
            query = query.Where(s => filter.MagicSchools.Contains(s.MagicSchool));

        if (filter.ClassIds is not null && filter.ClassIds.Count != 0)
            query = query.Where(s => s.Classes.Any(c => filter.ClassIds.Contains(c.Id)));

        if (filter.Durations is not null && filter.Durations.Count != 0)
            query = query.Where(s => filter.Durations.Contains(s.Duration));

        if (filter.CastingTimes is not null && filter.CastingTimes.Count != 0)
            query = query.Where(s => filter.CastingTimes.Contains(s.CastingTime));

        if (filter.SpellTypes is not null)
            query = query.Where(s => s.SpellTypes.Any(t => filter.SpellTypes.Contains(t)));

        if (filter.TargetType is not null && filter.TargetType.Count != 0)
            query = query.Where(s => filter.TargetType.Contains(s.SpellTargeting.TargetType));

        if (filter.Range is not null && filter.Range.Count != 0)
            query = query.Where(s => filter.Range.Contains(s.SpellTargeting.Range));

        if (filter.DamageTypes is not null)
            query = query.Where(s => s.DamageTypes.Any(t => filter.DamageTypes.Contains(t)));

        if (filter.IsHomebrew.HasValue)
            query = query.Where(s => s.IsHomebrew == filter.IsHomebrew.Value);

        return await query.ToListAsync();
    }
}