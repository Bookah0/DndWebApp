using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Characters;

public class CharacterRepository : ICharacterRepository
{
    private readonly AppDbContext context;
    private readonly IRepository<Character> baseRepo;

    public CharacterRepository(AppDbContext context, IRepository<Character> baseRepo)
    {
        this.context = context;
        this.baseRepo = baseRepo;
    }

    public async Task<Character> CreateAsync(Character entity) => await baseRepo.CreateAsync(entity);
    public async Task<Character?> GetByIdAsync(int id) => await baseRepo.GetByIdAsync(id);
    public async Task<ICollection<Character>> GetAllAsync() => await baseRepo.GetAllAsync();
    public async Task UpdateAsync(Character updatedEntity) => await baseRepo.UpdateAsync(updatedEntity);
    public async Task DeleteAsync(Character entity) => await baseRepo.DeleteAsync(entity);

    public async Task<CharacterSpellSlotsDto?> GetCurrentSpellSlotsAsync(int id)
    {
        return await context.Characters
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(r => new CharacterSpellSlotsDto
            {
                CharacterId = r.Id,
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
                AlignmentId = r.CharacterBuildData.AlignmentId,
                PersonalityTraits = r.CharacterBuildData.PersonalityTraits,
                Ideals = r.CharacterBuildData.Ideals,
                Bonds = r.CharacterBuildData.Bonds,
                Flaws = r.CharacterBuildData.Flaws,
                Age = r.CharacterBuildData.Age,
                Height = r.CharacterBuildData.Height,
                Weight = r.CharacterBuildData.Weight,
                Eyes = r.CharacterBuildData.Eyes,
                Skin = r.CharacterBuildData.Skin,
                Hair = r.CharacterBuildData.Hair,
                AlliesAndOrganizations = r.CharacterBuildData.AlliesAndOrganizations,
                Backstory = r.CharacterBuildData.Backstory,
                CharacterPictureUrl = r.CharacterBuildData.CharacterPictureUrl!
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