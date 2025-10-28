using DndWebApp.Api.Data;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Models.World;
using DndWebApp.Api.Repositories;
using DndWebApp.Api.Repositories.Abilities;
using DndWebApp.Api.Repositories.Skills;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services;

public class SkillService : IService<Skill, SkillDto, SkillDto>
{
    protected SkillRepository repo;
    protected AppDbContext context;
    protected AbilityRepository abilityRepo;
    public SkillService(SkillRepository repo, AbilityRepository abilityRepo, AppDbContext context)
    {
        this.context = context;
        this.repo = repo;
        this.abilityRepo = abilityRepo;
    }

    public async Task<Skill> CreateAsync(SkillDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        var ability = await abilityRepo.GetByIdAsync(dto.AbilityId) ?? throw new NullReferenceException("Ability could not be found");

        Skill skill = new()
        {
            Name = dto.Name,
            AbilityId = dto.AbilityId,
            Ability = ability,
            IsHomebrew = dto.IsHomebrew,
        };
        
        await repo.CreateAsync(skill);
        await context.SaveChangesAsync();
        return skill;
    }

    public async Task DeleteAsync(int id)
    {
        var skill = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Skill could not be found");
        await repo.DeleteAsync(skill);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Skill>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<ICollection<Skill>> GetAllWithAbilityAsync()
    {
        return await repo.GetAllWithAbilityAsync();
    }

    public async Task<Skill> GetByIdAsync(int id)
    {
        var skill = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Skill could not be found");
        return skill;
    }

    public async Task UpdateAsync(SkillDto dto)
    {
        ValidationUtil.ValidateRequiredString(dto.Name);
        var skill = await repo.GetByIdAsync(dto.Id) ?? throw new NullReferenceException("Skill could not be found");

        if (skill.AbilityId != dto.AbilityId)
        {
            skill.Ability = await abilityRepo.GetByIdAsync(dto.AbilityId) ?? throw new NullReferenceException("Ability could not be found");
            skill.AbilityId = dto.AbilityId;
        }

        skill.Name = dto.Name;

        await repo.UpdateAsync(skill);
        await context.SaveChangesAsync();
    }
    
    public enum SkillSorting { Name, Ability }
    public ICollection<Skill> SortBy(ICollection<Skill> skills, SkillSorting sortFilter, bool descending = false)
    {
        var abilityOrder = SortUtil.CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return sortFilter switch
        {
            SkillSorting.Name => SortUtil.OrderByMany(skills, [(s => s.Name)], descending),
            SkillSorting.Ability => SortUtil.OrderByMany(skills, [(s => abilityOrder[s.Ability!.FullName]), (s => s.Name)], descending),
            _ => skills,
        };
    }
}