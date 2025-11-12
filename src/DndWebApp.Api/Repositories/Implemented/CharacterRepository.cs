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

    public async Task<ICollection<Character>> GetMiscellaneousItemsAsync() => await context.Characters.ToListAsync();
    public async Task<Character?> GetByIdAsync(int id) => await context.Characters.FindAsync(id);

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

    public async Task<Character?> GetWithCombatStatsAsync(int id)
    {
        return await context.Characters
            .Include(c => c.CombatStats)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Character?> GetWithCharacterDescriptionAsync(int id)
    {
        return await context.Characters
            .Include(c => c.CharacterDescription)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Character?> GetWithAllDataAsync(int id)
    {
        return await context.Characters
            .Include(f => f.Class)
            .Include(f => f.SubClass)
            .Include(f => f.Background)
            .Include(f => f.Race)
            .Include(f => f.Subrace)
            .Include(c => c.Inventory)
            .Include(c => c.CombatStats)
            .Include(c => c.CharacterDescription)
            .AsSplitQuery()
            .Include(c => c.CurrentClassSlots)
            .Include(c => c.AbilityScores)
            .Include(c => c.SavingThrows)
            .Include(c => c.DamageAffinities)
            .Include(c => c.SkillProficiencies)
            .Include(c => c.WeaponCategoryProficiencies)
            .Include(c => c.WeaponTypeProficiencies)
            .Include(c => c.ArmorProficiencies)
            .Include(c => c.ToolProficiencies)
            .Include(c => c.Languages)
            .Include(f => f.OtherRaces)
            .Include(f => f.ReadySpells)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}