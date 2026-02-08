using Api.Data;
using Api.Models.Characters;
using Api.Models.DTOs.RequestDtos.Character;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Implemented;

public class CharacterRepository(AppDbContext context) : ICharacterRepository
{
    public async Task<Character> GetByIdAsync(int id) =>
        await context.Characters.FindAsync(id)
        ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<CharacterInfo> GetCharacterInfoAsync(int id) =>
        await context.Characters
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(r => new CharacterInfo
            {
                AlignmentId = r.Info.AlignmentId,
                PersonalityTraits = r.Info.PersonalityTraits,
                Ideals = r.Info.Ideals,
                Bonds = r.Info.Bonds,
                Flaws = r.Info.Flaws,
                Age = r.Info.Age,
                Height = r.Info.Height,
                Weight = r.Info.Weight,
                Eyes = r.Info.Eyes,
                Skin = r.Info.Skin,
                Hair = r.Info.Hair,
                AlliesAndOrganizations = r.Info.AlliesAndOrganizations,
                Backstory = r.Info.Backstory,
                CharacterPictureUrl = r.Info.CharacterPictureUrl!
            })
            .FirstOrDefaultAsync()
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithCombatStatsAsync(int id) =>
        await context.Characters
            .Include(c => c.CombatStats)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithCharacterInfoAsync(int id) =>
        await context.Characters
            .Include(c => c.Info)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithClassesAsync(int id) =>
        await context.Characters
            .Include(c => c.Class)
            .Include(c => c.SubClass)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<Character> GetWithFeaturesAsync(int id) =>
        await context.Characters
            .Include(c => c.Class)
                .ThenInclude(c => c.ClassLevels)
                    .ThenInclude(l => l.NewFeatures)
            .Include(c => c.SubClass)
                .ThenInclude(s => s!.ClassLevels)
                    .ThenInclude(l => l.NewFeatures)
            .Include(c => c.Race)
                    .ThenInclude(r => r.Traits)
            .Include(c => c.Subrace)
                .ThenInclude(s => s!.Traits)
            .Include(c => c.Background)
                .ThenInclude(b => b!.Features)
            .Include(c => c.Feats)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");


    public async Task<Character> GetWithAllDataAsync(int id) =>
        await context.Characters
            .Include(f => f.Class)
            .Include(f => f.SubClass)
            .Include(f => f.Background)
            .Include(f => f.Race)
            .Include(f => f.Subrace)
            .Include(c => c.Inventory)
            .Include(c => c.CombatStats)
            .Include(c => c.Info)
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
            .Include(f => f.ReadySpells)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception($"Character with id {id} could not be found");

    public async Task<ICollection<Character>> GetAllAsync() => await context.Characters.ToListAsync();

    public async Task<Character> CreateAsync(Character entity)
    {
        await context.Characters.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Character entity)
    {
        context.Characters.Remove(entity);
        await context.SaveChangesAsync();
    }
    public async Task<Character> UpdateAsync(Character updatedEntity)
    {
        context.Characters.Update(updatedEntity);
        await context.SaveChangesAsync();
        return updatedEntity;
    }
}