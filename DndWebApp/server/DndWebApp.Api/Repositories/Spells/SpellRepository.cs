using DndWebApp.Api.Data;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.Spells;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Spells;

public class SpellRepository(AppDbContext context) : EfRepository<Spell>(context), ISpellRepository
{
    public async Task<Spell?> GetWithClassesAsync(int id)
    {
        return await dbSet
            .Include(s => s.Classes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Spell>> GetAllWithClassesAsync()
    {
        return await dbSet
            .Include(s => s.Classes)
            .ToListAsync();
    }

    public async Task<ICollection<Spell>> FilterAsync(SpellFilter filter)
    {
        var query = dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(s => s.Name.Contains(filter.Name));

        if (filter.MinLevel.HasValue)
            query = query.Where(s => s.Level >= filter.MinLevel.Value);

        if (filter.MaxLevel.HasValue)
            query = query.Where(s => s.Level <= filter.MaxLevel.Value);

        if (filter.MagicSchools != null && filter.MagicSchools.Count != 0)
            query = query.Where(s => filter.MagicSchools.Contains(s.MagicSchool));

        if (filter.ClassIds != null && filter.ClassIds.Count != 0)
            query = query.Where(s => s.Classes.Any(c => filter.ClassIds.Contains(c.Id)));

        if (filter.Durations != null && filter.Durations.Count != 0)
            query = query.Where(s => filter.Durations.Contains(s.Duration));

        if (filter.CastingTimes != null && filter.CastingTimes.Count != 0)
            query = query.Where(s => filter.CastingTimes.Contains(s.CastingTime));

        if (filter.SpellTypes != null)
            query = query.Where(s => filter.SpellTypes.ToString()!.Contains(s.SpellTypes.ToString()));

        if (filter.TargetType != null && filter.TargetType.Count != 0)
             query = query.Where(s => filter.TargetType.Contains(s.SpellTargeting.TargetType));

        if (filter.Range != null && filter.Range.Count != 0)
            query = query.Where(s => filter.Range.Contains(s.SpellTargeting.Range));

        if (filter.DamageTypes != null)
            query = query.Where(s => filter.DamageTypes.ToString()!.Contains(s.SpellDamage.DamageTypes.ToString()));

        if (filter.IsHomebrew.HasValue)
            query = query.Where(s => s.IsHomebrew == filter.IsHomebrew.Value);
    
        return await query.ToListAsync();
    }
}