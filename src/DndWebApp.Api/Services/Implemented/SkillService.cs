using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Models.DTOs;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Enums;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;
namespace DndWebApp.Api.Services.Implemented;

public class SkillService : ISkillService
{
    private readonly ISkillRepository repo;
    private readonly IAbilityRepository abilityRepo;
    private readonly ILogger<SkillService> logger;
    
    public SkillService(ISkillRepository repo, IAbilityRepository abilityRepo, ILogger<SkillService> logger)
    {
        this.repo = repo;
        this.abilityRepo = abilityRepo;
        this.logger = logger;
    }

    public async Task<Skill> CreateAsync(SkillDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        var ability = await abilityRepo.GetByIdAsync(dto.AbilityId) ?? throw new NullReferenceException("Ability could not be found");

        Skill skill = new()
        {
            Name = dto.Name,
            AbilityId = dto.AbilityId,
            Ability = ability,
            IsHomebrew = dto.IsHomebrew,
        };

        return await repo.CreateAsync(skill);
    }

    public async Task DeleteAsync(int id)
    {
        var skill = await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Skill could not be found");
        await repo.DeleteAsync(skill);
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
        return await repo.GetByIdAsync(id) ?? throw new NullReferenceException("Skill could not be found");
    }

    public async Task UpdateAsync(SkillDto dto)
    {
        ValidationUtil.HasContentOrThrow(dto.Name);
        ValidationUtil.AboveZeroOrThrow(dto.AbilityId);
        
        var skill = await repo.GetByIdAsync(dto.Id) 
            ?? throw new NullReferenceException("Skill could not be found");

        if (skill.AbilityId != dto.AbilityId)
        {
            skill.Ability = await abilityRepo.GetByIdAsync(dto.AbilityId)
                ?? throw new NullReferenceException("Ability could not be found");
            skill.AbilityId = dto.AbilityId;
        }

        skill.Name = dto.Name;
        skill.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(skill);
    }

    
    public ICollection<Skill> SortBy(ICollection<Skill> skills, SkillSortFilter SortFilter, bool descending = false)
    {
        var abilityOrder = SortUtil.CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return SortFilter switch
        {
            SkillSortFilter.Name => SortUtil.OrderByMany(skills, [(s => s.Name)], descending),
            SkillSortFilter.Ability => SortUtil.OrderByMany(skills, [(s => abilityOrder[s.Ability!.FullName]), (s => s.Name)], descending),
            _ => skills,
        };
    }
}