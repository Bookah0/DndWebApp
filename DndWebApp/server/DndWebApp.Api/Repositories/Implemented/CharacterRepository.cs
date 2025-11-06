using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Implemented;

public class CharacterRepository : ICharacterRepository
{
    private readonly AppDbContext context;

    public CharacterRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Character> CreateAsync(Character entity)
    {
        await context.Characters.AddAsync(entity!);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Character entity)
    {
        context.Characters.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Character updatedEntity)
    {
        context.Characters.Update(updatedEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Character>> GetAllAsync() => await context.Characters.ToListAsync();
    public async Task<Character?> GetByIdAsync(int id) => await context.Characters.FindAsync(id);

    public async Task<CurrentSpellSlots?> GetCurrentSpellSlotsAsync(int id)
    {
        return await context.Characters
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(r => new CurrentSpellSlots
            {
                Lvl1 = r.CurrentSpellSlots!.Lvl1,
                Lvl2 = r.CurrentSpellSlots.Lvl2,
                Lvl3 = r.CurrentSpellSlots.Lvl3,
                Lvl4 = r.CurrentSpellSlots.Lvl4,
                Lvl5 = r.CurrentSpellSlots.Lvl5,
                Lvl6 = r.CurrentSpellSlots.Lvl6,
                Lvl7 = r.CurrentSpellSlots.Lvl7,
                Lvl8 = r.CurrentSpellSlots.Lvl8,
                Lvl9 = r.CurrentSpellSlots.Lvl9
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CharacterDescriptionDto?> GetCharacterDescriptionAsync(int id)
    {
        return await context.Characters
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(r => new CharacterDescriptionDto
            {
                AlignmentId = r.CharacterDescription.AlignmentId,
                PersonalityTraits = r.CharacterDescription.PersonalityTraits,
                Ideals = r.CharacterDescription.Ideals,
                Bonds = r.CharacterDescription.Bonds,
                Flaws = r.CharacterDescription.Flaws,
                Age = r.CharacterDescription.Age,
                Height = r.CharacterDescription.Height,
                Weight = r.CharacterDescription.Weight,
                Eyes = r.CharacterDescription.Eyes,
                Skin = r.CharacterDescription.Skin,
                Hair = r.CharacterDescription.Hair,
                AlliesAndOrganizations = r.CharacterDescription.AlliesAndOrganizations,
                Backstory = r.CharacterDescription.Backstory,
                CharacterPictureUrl = r.CharacterDescription.CharacterPictureUrl!
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Character?> GetWithAllDataAsync(int id)
    {
        return await context.Characters
            .Include(f => f.Class)
            .Include(f => f.SubClass)
            .Include(f => f.Background)
            .Include(f => f.Race)
            .AsSplitQuery()
            .Include(f => f.OtherRaces)
            .Include(f => f.ReadySpells)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}