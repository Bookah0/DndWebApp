using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Abilities;
using Microsoft.EntityFrameworkCore;

namespace DndWebApp.Api.Repositories.Characters;

public class CharacterRepository(AppDbContext context) : EfRepository<Character>(context), ICharacterRepository
{
    public async Task<CharacterSpellSlotsDto?> GetCurrentSpellSlotsAsync(int id)
    {
        return await dbSet
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
        return await dbSet
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

    public async Task<PrimitiveCharacterDto?> GetPrimitiveDataAsync(int id)
    {
        return await dbSet
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(r => new PrimitiveCharacterDto
            {
                Id = r.Id,
                Name = r.Name,
                Level = r.Level,
                RaceId = r.RaceId,
                ClassId = r.ClassId,
                SubClassId = r.SubClassId,
                BackgroundId = r.BackgroundId,
                Experience = r.Experience,
                PlayerName = r.PlayerName,
                ProficiencyBonus = r.ProficiencyBonus,
                MaxHP = r.CombatStats.MaxHP,
                CurrentHP = r.CombatStats.CurrentHP,
                TempHP = r.CombatStats.TempHP,
                ArmorClass = r.CombatStats.ArmorClass,
                Initiative = r.CombatStats.Initiative,
                Speed = r.CombatStats.Speed,
                MaxHitDice = r.CombatStats.MaxHitDice,
                CurrentHitDice = r.CombatStats.CurrentHitDice
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<PrimitiveCharacterDto>> GetAllPrimitiveDataAsync()
    {
        return await dbSet
            .AsNoTracking()
            .Select(r => new PrimitiveCharacterDto
            {
                Id = r.Id,
                Name = r.Name,
                Level = r.Level,
                RaceId = r.RaceId,
                ClassId = r.ClassId,
                SubClassId = r.SubClassId,
                BackgroundId = r.BackgroundId,
                Experience = r.Experience,
                PlayerName = r.PlayerName,
                ProficiencyBonus = r.ProficiencyBonus,
                MaxHP = r.CombatStats.MaxHP,
                CurrentHP = r.CombatStats.CurrentHP,
                TempHP = r.CombatStats.TempHP,
                ArmorClass = r.CombatStats.ArmorClass,
                Initiative = r.CombatStats.Initiative,
                Speed = r.CombatStats.Speed,
                MaxHitDice = r.CombatStats.MaxHitDice,
                CurrentHitDice = r.CombatStats.CurrentHitDice
            })
            .ToListAsync();
    }

    public async Task<Character?> GetWithAllDataAsync(int id)
    {
        return await dbSet
            .Include(f => f.Class)
            .Include(f => f.SubClass)
            .Include(f => f.Background)
            .Include(f => f.Race)
            .Include(f => f.AbilityScores)
            .Include(f => f.CombatStats)
            .Include(f => f.CurrentSpellSlots)
            .Include(f => f.CharacterBuildData)
            .AsSplitQuery()
            .Include(f => f.OtherRaces)
            .Include(f => f.PassiveEffects)
            .Include(f => f.ReadySpells)
            .Include(f => f.SavingThrows)
            .Include(f => f.DamageAffinities)
            .Include(f => f.SkillProficiencies)
            .Include(f => f.WeaponProficiencies)
            .Include(f => f.ArmorProficiencies)
            .Include(f => f.ToolProficiencies)
            .Include(f => f.Languages)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}