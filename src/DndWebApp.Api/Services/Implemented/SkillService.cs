using DndWebApp.Api.Middlewares.ExceptionHandling;
using DndWebApp.Api.Models.Characters;
using DndWebApp.Api.Repositories.Interfaces;
using DndWebApp.Api.Services.Constants;
using DndWebApp.Api.Services.Interfaces;
using DndWebApp.Api.Services.Util;
using static DndWebApp.Api.Services.Util.SortUtil;
using DndWebApp.Api.Models.DTOs.RequestDtos.Character;

namespace DndWebApp.Api.Services.Implemented;

public class SkillService(ISkillRepository repo, IAbilityRepository abilityRepo, ILogger<SkillService> logger) : ISkillService
{
    public async Task<Skill> CreateAsync(SkillDto dto)
    {
        var ability = await abilityRepo.GetByIdAsync(dto.AbilityId) ?? throw new NotFoundException("Ability could not be found");

        var skill = await repo.CreateAsync(new()
        {
            Name = dto.Name,
            AbilityId = dto.AbilityId,
            Ability = ability,
            IsHomebrew = dto.IsHomebrew,
        });
        return skill;
    }

    public async Task DeleteAsync(int id)
    {
        var skill = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Skill could not be found");
        await repo.DeleteAsync(skill);
    }

    public async Task<ICollection<Skill>> GetAllAsync()
    {
        var skills = await repo.GetAllAsync();
        return skills;
    }

    public async Task<ICollection<Skill>> GetAllWithAbilityAsync()
    {
        var skills = await repo.GetAllWithAbilityAsync();
        return skills;
    }

    public async Task<Skill> GetByIdAsync(int id)
    {
        var skill = await repo.GetByIdAsync(id) ?? throw new NotFoundException("Skill could not be found");
        return skill;
    }

    public async Task UpdateAsync(int id, SkillDto dto)
    {
        var skill = await repo.GetByIdAsync(id) 
            ?? throw new NotFoundException("Skill could not be found");

        if (skill.AbilityId != dto.AbilityId)
        {
            skill.Ability = await abilityRepo.GetByIdAsync(dto.AbilityId)
                ?? throw new NotFoundException("Ability could not be found");
            skill.AbilityId = dto.AbilityId;
        }

        skill.Name = dto.Name;
        skill.IsHomebrew = dto.IsHomebrew;

        await repo.UpdateAsync(skill);
    }

    public ICollection<Skill> SortBy(ICollection<Skill> skills, string sortFilter, bool descending = false)
    {
        if(!ConstantsUtil.TryResolveOption(sortFilter, SortSkillOption.AllowedValues, out string? resolved))
            return skills;

        var abilityOrder = CreateOrderLookup(["Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma"]);

        return resolved switch
        {
            SortSkillOption.Name => OrderByMany(skills, [(s => s.Name)], descending),
            SortSkillOption.Ability => OrderByMany(skills, [(s => abilityOrder[s.Ability!.FullName]), (s => s.Name)], descending),
            _ => throw new ArgumentOutOfRangeException(nameof(sortFilter), "Invalid sort option provided.")
        };
    }
}